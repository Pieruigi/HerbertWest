using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class FifteenPuzzleController : PuzzleController
    {
       

        [System.Serializable]
        class Tile
        {
            public GameObject tileObject;
            public int row, col;
        }

        [SerializeField]
        List<Tile> tiles;

        [SerializeField]
        Collections.Item missingTileItem;

        [SerializeField]
        GameObject cover;

        [SerializeField]
        GameObject tileContainer;

        [SerializeField]
        Picker picker;

        Vector3[] positions; // Default positions

        int missingTileId = 15;

        int openState = 0;

        Vector3[] mixedPositions;
        int freePositionId = -1;

        bool interacting = false;

        float coverMoveDisp = 0.5f;

        protected override void Awake()
        {
            base.Awake();

            // Init default positions
            positions = new Vector3[tiles.Count];
            for (int i = 0; i < positions.Length; i++)
            {
                // Store physical position
                positions[i] = tiles[i].tileObject.transform.position;

                // Set row and column
                int row, col;
                MathUtility.ArrayIndexToMatrixCoords(i, 4, out row, out col);
                tiles[i].row = row;
                tiles[i].col = col;
            }


            // Set the finite state machine handle to manage the missing tile
            finiteStateMachine.OnStateChange += HandleOnStateChangeExt;    
                
        }

        protected override void Start()
        {
            base.Start();

            // Check state
            if(finiteStateMachine.CurrentStateId == openState)
            {
                // Open the box
                Vector3 pos = cover.transform.position;
                pos.x += coverMoveDisp;
                cover.transform.position = pos;
                pos = tileContainer.transform.position;
                pos.x += coverMoveDisp;
                tileContainer.transform.position = pos;

                // Set picker as picked
                picker.SetSceneObjectAsPicked();
            }
            else if(finiteStateMachine.CurrentStateId == CompletedState)
            {
                // We must put the missing tile yet, but the puzzle has been resolved
                // Remove the missing piece
                tiles[missingTileId].tileObject.SetActive(false);
                freePositionId = missingTileId;
                //freePositionId = 8;
            }
            else
            {
                // Puzzle has not been solved yet, so we must mix the tiles
                // First create a list of indices from 0 to 15
                //List<int> indices = new List<int>();
                //for(int i=0; i<tiles.Count; i++)
                //    indices.Add(i);
                List<int> indices = GetStartingIndices();
                
                // Now get a new free position for each tile
                for(int i=0; i<tiles.Count; i++)
                {
                    // Get a random position
                    //int posId = indices[Random.Range(0, indices.Count)];
                    //indices.Remove(posId);

                    int posId = indices[i];

                    // Set tile new position
                    MathUtility.ArrayIndexToMatrixCoords(posId, 4, out tiles[i].row, out tiles[i].col);
                    // Set new physical position
                    tiles[i].tileObject.transform.position = positions[posId];
                    
                }
                // Remove the missing tile
                tiles[missingTileId].tileObject.SetActive(false);
                int row = tiles[missingTileId].row;
                int col = tiles[missingTileId].col;
                freePositionId = MathUtility.MatrixCoordsToArrayIndex(row, col, 4);
            }

            
        }

        List<int> GetStartingIndices()
        {
            List<int> indices = new List<int>();
            indices.Add(0);
            indices.Add(5);
            indices.Add(10);
            indices.Add(13);
            indices.Add(14);
            indices.Add(7);
            indices.Add(9);
            indices.Add(11);
            indices.Add(4);
            indices.Add(3);
            indices.Add(12);
            indices.Add(6);
            indices.Add(1);
            indices.Add(2);
            indices.Add(8);
            indices.Add(15);

            return indices;
        }

        public override void Interact(Interactor interactor)
        {
            if (interacting)
                return;

            interacting = true;

            StartCoroutine(DoInteraction(interactor));
        }

        IEnumerator DoInteraction(Interactor interactor)
        {
            OnPuzzleInteractionStart?.Invoke(this);

            Debug.Log("Interaction:" + interactor);

            // Get the tile we are clicking on
            Tile tile = tiles.Find(t => t.tileObject == interactor.gameObject);
           
            if (TryMoveTile(tile)) 
            {
                // Move tile
                float time = 0.5f;
                Vector3 newPos = positions[MathUtility.MatrixCoordsToArrayIndex(tile.row, tile.col, 4)];
                LeanTween.move(tile.tileObject, newPos, time);//.setEaseInOutExpo();

                // Move the empty tile
                newPos = positions[MathUtility.MatrixCoordsToArrayIndex(tiles[missingTileId].row, tiles[missingTileId].col, 4)];
                tiles[missingTileId].tileObject.transform.position = newPos;

                yield return new WaitForSeconds(time);

                if (IsSolved())
                {
                    
                    SetStateCompleted();
                    GetComponent<Messenger>().SendInGameMessage(11);
                    interacting = false;
                    Exit();
                }
            }
                

            interacting = false;

            OnPuzzleInteractionStop?.Invoke(this);

        }

        /// <summary>
        /// Try move will update row and column if tile can be moved
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        bool TryMoveTile(Tile tile)
        {
            
            // Get the position id
            int posId = MathUtility.MatrixCoordsToArrayIndex(tile.row, tile.col, 4);
            // This is the missing tile
            if (posId == freePositionId)
                return false;

            // Check if we can move it
            // Check north
            if(posId - 4 == freePositionId)
            {
                // Update the missing tile position
                tiles[missingTileId].row = tile.row;
                tiles[missingTileId].col = tile.col;
                // Update current tile
                tile.row -= 1;
                // Update the free position
                freePositionId = posId;
                
                
                return true;
            }
            // Check east
            if (posId + 1 == freePositionId)
            {
                tiles[missingTileId].row = tile.row;
                tiles[missingTileId].col = tile.col;
                tile.col += 1;
                freePositionId = posId;
                return true;
            }
            // Check south
            if (posId + 4 == freePositionId)
            {
                tiles[missingTileId].row = tile.row;
                tiles[missingTileId].col = tile.col;
                tile.row += 1;
                freePositionId = posId;
                return true;
            }
            // Check west
            if (posId - 1 == freePositionId)
            {
                tiles[missingTileId].row = tile.row;
                tiles[missingTileId].col = tile.col;
                tile.col-=1;
                freePositionId = posId;
                return true;
            }


            return false;
        }

        bool IsSolved()
        {
            
            // Check if all the tiles are in the right order
            for(int i=0; i<tiles.Count; i++)
            {
                // Get the actual tile position
                int posId = MathUtility.MatrixCoordsToArrayIndex(tiles[i].row, tiles[i].col, 4);

                // Check for the target position, which is 'i'
                if (posId != i)
                    return false;
            }

            return true;
        }

        void HandleOnStateChangeExt(FiniteStateMachine fsm)
        {
            if (interacting)
                return;

            if (fsm.CurrentStateId == CompletedState)
            {
                // Open the inventory
                UI.InventoryUI.Instance.OnItemChosen += HandleOnItemChosen;
                GameManager.Instance.OpenInventory(true);
                
            }
        }

        IEnumerator PutMissingTile()
        {
            // Set the tile visible
            tiles[missingTileId].tileObject.SetActive(true);

            yield return new WaitForSeconds(1f);

            // Remove the item from the inventory
            Inventory.Instance.Remove(missingTileItem);

            // Open top
            float time = 0.5f;
            LeanTween.moveX(cover, cover.transform.position.x + coverMoveDisp, time);
            LeanTween.moveX(tileContainer, tileContainer.transform.position.x + coverMoveDisp, time);

            // Take whatever is inside
            yield return picker.Pick();
        }

        void HandleOnItemChosen(Collections.Item item)
        {
            if (IsWrongItem(item))
            {
                UI.InventoryUI.Instance.ShowWrongItemMessage();
            }
            else
            {
                // Set open state
                finiteStateMachine.ForceState(0, true, true);

                // Put the missing tile
                StartCoroutine(PutMissingTile());

                // Close inventory
                UI.InventoryUI.Instance.OnItemChosen -= HandleOnItemChosen;
                UI.InventoryUI.Instance.Close();
            }
        }

        bool IsWrongItem(Collections.Item item)
        {
            return item != missingTileItem;
        }
    }

}
