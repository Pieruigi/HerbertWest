using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class ChessPuzzleController : PuzzleController
    {

        [SerializeField]
        GameObject horse;

        [SerializeField]
        List<GameObject> enemies;

        [SerializeField]
        Transform chessContainer;

        bool interacting = false;

        List<GameObject> tiles;
        List<GameObject> activeTiles = new List<GameObject>(); // All the active tiles
        List<GameObject> deadEnemies = new List<GameObject>();

        int size = 8;

        float downDisp = 0.018f;
        float upDisp = 0.02f;

        float defaultY;
        bool started = false;

        // Tiles you must lower down
        List<GameObject> lowerDownList = new List<GameObject>();



        protected override void Start()
        {
            base.Start();

            if(finiteStateMachine.CurrentStateId == CompletedState)
            {
                // Set the chess open
            }
            else
            {
                // Set default y
                defaultY = horse.transform.position.y;

                // Fill chess tiles
                tiles = new List<GameObject>();
                for(int i=0; i<chessContainer.childCount; i++)
                {
                    Transform tile = chessContainer.GetChild(i);
                    tiles.Add(tile.gameObject);

                    // All tiles are not interactable but the horse
                    if(tile.gameObject == horse)
                        activeTiles.Add(tile.gameObject);
                    
                }

                // Init horse
                Vector3 pos = horse.transform.position;
                pos.y += upDisp;
                horse.transform.position = pos;

                // Init enemies
                for(int i=0; i<enemies.Count; i++)
                {
                    pos = enemies[i].transform.position;
                    pos.y -= downDisp;
                    
                    enemies[i].transform.position = pos;
                }
            }
        }

       
        public override void Interact(Interactor interactor)
        {
            if (interacting)
                return;

            if (!activeTiles.Contains(interactor.gameObject))
                return;
 
            interacting = true;

            Debug.Log("Interacting with " + interactor.gameObject);

            UpdateTiles(interactor);

            StartCoroutine(DoInteraction());
        }

        

        IEnumerator DoInteraction()
        {
            OnPuzzleInteractionStart?.Invoke(this);

            MoveTiles();

            yield return new WaitForSeconds(1f);

            interacting = false;

            OnPuzzleInteractionStop?.Invoke(this);
        }

        void UpdateTiles(Interactor interactor)
        {
            
            if(interactor.gameObject == horse)
            {
                // Check whether the puzzle has started or not
                if (!started)
                {
                    // We just started the puzzle ( first time we press the horse )
                    started = true;

                    lowerDownList.Clear();
                    lowerDownList.Add(horse);
                    // Reset horse position
                    //LeanTween.moveY(interactor.gameObject, defaultY, 0.5f);

                    //// Move up enemies

                    //foreach (GameObject enemy in enemies)
                    //    LeanTween.moveY(enemy, defaultY, 0.5f);

                    // Get the index of the horse
                    //int pressedIndex = tiles.IndexOf(horse);

                    // Update the active tile list
                    UpdateActiveTileList(interactor.gameObject);

                   
   
                }
            }

            
        }

        void MoveTiles()
        {

            // Lower down 
            foreach (GameObject tile in lowerDownList)
            {
                LeanTween.moveY(tile, defaultY, 0.5f);
            }

            // Clear the list
            lowerDownList.Clear();

            // Move up available tiles
            foreach (GameObject tile in activeTiles)
            {
                LeanTween.moveY(tile, defaultY+upDisp, 0.5f);
            }

        }

        void UpdateActiveTileList(GameObject pressedTile)
        {
            int pressedIndex = tiles.IndexOf(horse); 

            // Clear 
            activeTiles.Clear();

            //
            // Calculate available tiles
            //

            // Get row and column
            int row, column;
            MathUtility.ArrayIndexToMatrixCoords(pressedIndex, size, out row, out column);

            //
            // Check tiles that the horse can reach
            //
            
            int targetRow, targetColumn;
            // Up left and right
            targetRow = row - 2;
            targetColumn = column - 1;
            if (CheckTileIsAvailable(targetRow, targetColumn))
                activeTiles.Add(tiles[MathUtility.MatrixCoordsToArrayIndex(targetRow, targetColumn, size)]);
            targetColumn = column + 1;
            if (CheckTileIsAvailable(targetRow, targetColumn))
                activeTiles.Add(tiles[MathUtility.MatrixCoordsToArrayIndex(targetRow, targetColumn, size)]);
            // Right up and down
            targetRow = row - 1;
            targetColumn = column + 2;
            if (CheckTileIsAvailable(targetRow, targetColumn))
                activeTiles.Add(tiles[MathUtility.MatrixCoordsToArrayIndex(targetRow, targetColumn, size)]);
            targetRow = row + 1;
            if (CheckTileIsAvailable(targetRow, targetColumn))
                activeTiles.Add(tiles[MathUtility.MatrixCoordsToArrayIndex(targetRow, targetColumn, size)]);
            // Down right and left
            targetRow = row + 2;
            targetColumn = column + 1;
            if (CheckTileIsAvailable(targetRow, targetColumn))
                activeTiles.Add(tiles[MathUtility.MatrixCoordsToArrayIndex(targetRow, targetColumn, size)]);
            targetColumn = column - 1;
            if (CheckTileIsAvailable(targetRow, targetColumn))
                activeTiles.Add(tiles[MathUtility.MatrixCoordsToArrayIndex(targetRow, targetColumn, size)]);
            // Left down and up
            targetRow = row + 1;
            targetColumn = column - 2;
            if (CheckTileIsAvailable(targetRow, targetColumn))
                activeTiles.Add(tiles[MathUtility.MatrixCoordsToArrayIndex(targetRow, targetColumn, size)]);
            targetRow = row - 1;
            if (CheckTileIsAvailable(targetRow, targetColumn))
                activeTiles.Add(tiles[MathUtility.MatrixCoordsToArrayIndex(targetRow, targetColumn, size)]);

           
        }

        bool CheckTileIsAvailable(int row, int column)
        {
            // Check if the tile is inside the chess
            if (row < 0 || row >= size || column < 0 || column >= size)
                return false;

            // Get the tile as array index 
            int index = MathUtility.MatrixCoordsToArrayIndex(row, column, size);

            // Check if is a dead enemy ( is tower? is bishop? ecc. )
            if (deadEnemies.Contains(tiles[index]))
            {
                // Yes is a dead enemy, return false
                return false;
            }

            return true;
        }
    }

}
