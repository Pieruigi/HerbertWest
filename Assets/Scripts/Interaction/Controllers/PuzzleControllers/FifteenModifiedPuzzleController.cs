using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class FifteenModifiedPuzzleController : PuzzleController
    {
        // Internal class useful to store row and column of each tile.
        [System.Serializable]
        class Tile
        {
            public GameObject tileObject;
            public int row, col;
        }

        [SerializeField]
        Transform tileContainer;


        List<Tile> tiles;
 
        bool interacting = false;
        int sizeH = 4, sizeV = 3;
        float moveTime = 0.5f;
        float moveDisp = 0.103f;

        protected override void Start()
        {
            base.Start();

            // Fill tiles array
            tiles = new List<Tile>();
            for (int i = 0; i < tileContainer.childCount; i++)
            {
                // Create tile
                Tile tile = new Tile();
                tile.tileObject = tileContainer.GetChild(i).gameObject;
                tile.row = i / 3 + 1; // Initial row 
                tile.col = i % 3 + 1; // Initial column
                // Add to the list
                tiles.Add(tile);

            }
                

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

            Debug.Log("Pressed " + interactor.name);

            // Press button
            StartCoroutine(PressButton(interactor.transform.GetChild(0).gameObject));

            // Split interactor name according to its format
            string[] splits = interactor.name.Split('-');

            if("east".Equals(splits[0].ToLower()) || "west".Equals(splits[0].ToLower()))
            {
                // Try move row
                if(TryMoveRow("east".Equals(splits[0].ToLower()), int.Parse(splits[1])))
                {
                    Debug.Log("Moving east or west...");
                    yield return new WaitForSeconds(moveTime+0.2f);
                }
                else
                {
                    Debug.Log("Can't move");
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else // Is north or south
            {
                if (TryMoveColumn("north".Equals(splits[0].ToLower()), int.Parse(splits[1])))
                {
                    Debug.Log("Moving north or south...");
                    yield return new WaitForSeconds(moveTime + 0.2f);
                }
                else
                {
                    Debug.Log("Can't move");
                    yield return new WaitForSeconds(0.1f);
                }
            }

            if (CheckCompleted())
            {
                SetStateCompleted(); // We should open the door at this point.
                yield return new WaitForSeconds(0.5f);
                GetComponent<Messenger>().SendInGameMessage(10);
                yield return new WaitForSeconds(1f);
                Exit();
            }



            interacting = false;

            OnPuzzleInteractionStop?.Invoke(this);
        }

        /// <summary>
        /// Tries to move a row east or west ( depending on the first parameter ).
        /// </summary>
        /// <param name="east">true to move east, false to move west</param>
        /// <param name="index">the index of the row</param>
        /// <returns></returns>
        bool TryMoveRow(bool east, int index)
        {
            // Get all the tiles in the given row.
            List<Tile> moveList = tiles.FindAll(t => t.row == index);

            // If one of the tiles is alredy on the edge we can't move.
            if ((east && moveList.Exists(t => t.col == sizeH - 1)) || (!east && moveList.Exists(t => t.col == 0)))
                return false;

            // Move tiles
            int dir = 1;
            if (!east)
                dir = -1;

            // Move all tiles
            foreach(Tile tile in moveList)
            {
                tile.col += dir;
                float x = tile.tileObject.transform.position.x + dir * moveDisp;
                LeanTween.moveX(tile.tileObject, x, moveTime).setEaseInOutExpo();
                
            }

            return true;
        }

        /// <summary>
        /// Tries to move a column north or south ( depending on the first parameter ).
        /// </summary>
        /// <param name="east">true to move north, false to move south</param>
        /// <param name="index">the index of the column</param>
        /// <returns></returns>
        bool TryMoveColumn(bool north, int index)
        {
            // Get all the tiles in the given colum.
            List<Tile> moveList = tiles.FindAll(t => t.col == index);

            // If one of the tiles is alredy on the edge we can't move.
            if ((north && moveList.Exists(t => t.row == 0)) || (!north && moveList.Exists(t => t.row == sizeV-1)))
                return false;

            // Move tiles
            int dir = 1;
            if (!north)
                dir = -1;

            // Move all tiles
            foreach (Tile tile in moveList)
            {
                tile.row -= dir;
                float z = tile.tileObject.transform.position.z + dir * moveDisp;
                LeanTween.moveZ(tile.tileObject, z, moveTime).setEaseInOutExpo();
               
            }

            return true;
        }

        bool CheckCompleted()
        {
            // We simply check row and column for each tile
            if (tiles[0].row != 1 || tiles[0].col != 3)
                return false;
            if (tiles[1].row != 2 || tiles[1].col != 2)
                return false;
            if (tiles[2].row != 2 || tiles[2].col != 1)
                return false;
            if (tiles[3].row != 1 || tiles[3].col != 1)
                return false;
            if (tiles[4].row != 2 || tiles[4].col != 3)
                return false;
            if (tiles[5].row != 1 || tiles[5].col != 2)
                return false;

            return true;
        }

        IEnumerator PressButton(GameObject button)
        {
            float yDef = button.transform.position.y;
            float yNew = yDef - 0.01f;
            
            float time = 0.2f;
            LeanTween.moveY(button, yNew, time).setEaseOutExpo();
            yield return new WaitForSeconds(time);
            LeanTween.moveY(button, yDef, time).setEaseOutExpo();
        }
    }

}
