using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;

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
        List<GameObject> lowerDownList = new List<GameObject>();

        int size = 8;

        float downDisp = 0.018f;
        float upDisp = 0.02f;
        float moveTime = 0.25f;

        float defaultY;
        bool started = false;

        


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
                //for(int i=0; i<enemies.Count; i++)
                //{
                //    pos = enemies[i].transform.position;
                //    pos.y -= downDisp;
                    
                //    enemies[i].transform.position = pos;
                //}
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

            StartCoroutine(DoInteraction(interactor));
        }

        

        IEnumerator DoInteraction(Interactor interactor)
        {
            OnPuzzleInteractionStart?.Invoke(this);

            UpdateTiles(interactor);

            MoveTiles();

            yield return new WaitForSeconds(0.3f);

            if (YouFailed(interactor))
            {
                yield return new WaitForSeconds(1f);
                GetComponent<Messenger>().SendInGameMessage(6);
                ResetTiles();
            }
            else
            {
                if (YouSucceded())
                {
                    SetStateCompleted();

                    yield return new WaitForSeconds(1f);
                    OpenChest();
                }
                    
            }

            interacting = false;

            OnPuzzleInteractionStop?.Invoke(this);
        }

        bool YouFailed(Interactor interactor)
        {
            //
            // We must check all directions to check if some enemy can eat you
            //
            int pushedIndex = tiles.IndexOf(interactor.gameObject);
            int row, col;

            // Get row and column of the pushed object
            MathUtility.ArrayIndexToMatrixCoords(pushedIndex, size, out row, out col);

            if (EatByTower(row, col))
            {
                Debug.Log("Eat by tower");
                return true;
            }
                

            if (EatByPawnOrBishop(row, col))
            {
                Debug.Log("Eat by pawn or bishop");
                return true;
            }
                
           

            return false;
        }

        bool YouSucceded()
        {
            return deadEnemies.Count == enemies.Count;
        }

        void ResetTiles()
        {
            // Clear all
            deadEnemies.Clear();
            activeTiles.Clear();
            lowerDownList.Clear();

            // Set the horse as interactable
            activeTiles.Add(horse);

            // Reset position for each tile
            foreach(GameObject tile in tiles)
            {
                if(tile != horse)
                    LeanTween.moveY(tile, defaultY, moveTime).setEaseInExpo();
                else
                    LeanTween.moveY(tile, defaultY+upDisp, moveTime).setEaseInExpo();
            }
        }

        void OpenChest()
        {

        }

        void UpdateTiles(Interactor interactor)
        {
            
            
            // Check whether the puzzle has started or not
            if (!started)
            {
                // We are just starting ( first time we press the horse )
                started = true;

                // Add the horse to the lowe down list
                lowerDownList.Clear();
                lowerDownList.Add(horse);
                   
                // Update the active tile list
                UpdateActiveTileList(interactor.gameObject);
            }
            else
            {
                // If you pushed an enemy tile then set it as dead
                if (enemies.Contains(interactor.gameObject))
                {
                    if("tower".Equals(interactor.gameObject.name.ToLower()))
                        GetComponent<Messenger>().SendInGameMessage(7);
                    else if("bishop".Equals(interactor.gameObject.name.ToLower()))
                        GetComponent<Messenger>().SendInGameMessage(8);
                    else
                        GetComponent<Messenger>().SendInGameMessage(9);

                    deadEnemies.Add(interactor.gameObject);
                }
                    

                // Put all the active tiles in the lower down list
                lowerDownList.Clear();
                foreach (GameObject tile in activeTiles)
                    lowerDownList.Add(tile);

                // Update the active tile list
                UpdateActiveTileList(interactor.gameObject);
            }
            

            
        }

        void MoveTiles()
        {

            // Lower down tiles 
            foreach (GameObject tile in lowerDownList)
            {
                if(deadEnemies.Contains(tile))
                {
                    // Is a dead enemy, put it really down
                    LeanTween.moveY(tile, defaultY-downDisp, moveTime).setEaseOutExpo();
                }
                else
                {
                    // Simple tile, put just down
                    LeanTween.moveY(tile, defaultY, moveTime).setEaseOutExpo();
                }
                
            }

            // Clear the list
            lowerDownList.Clear();

            // Move up available tiles
            foreach (GameObject tile in activeTiles)
            {
                LeanTween.moveY(tile, defaultY+upDisp, moveTime).setEaseInExpo();
            }

        }

        void UpdateActiveTileList(GameObject pressedTile)
        {
            int pressedIndex = tiles.IndexOf(pressedTile); 

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

        bool EatByTower(int row, int col)
        {
            for(int k=0; k<1; k++)
            {
             
                for(int i=0; i<size; i++)
                {
                    int tmpRow;
                    if (k == 0)
                    {
                        // North
                        tmpRow = row - 1 - i;
                        if (tmpRow < 0)
                            break;
                    }
                    else
                    {
                        // South
                        tmpRow = row + 1 + i;
                        if (tmpRow >= size)
                            break;
                    }
                    // Get the index of the tile we are checking
                    Debug.LogFormat("Checking tile [{0},{1}]", tmpRow, col);
                    int tmpIndex = MathUtility.MatrixCoordsToArrayIndex(tmpRow, col, size);
                    // If the tile is a tower and it's not dead you die
                    if ("tower".Equals(tiles[tmpIndex].name.ToLower()) && !deadEnemies.Contains(tiles[tmpIndex]))
                    {
                        return true;
                    }

                    // If tile is an enemy ( but not a tower ) and is not dead you get some cove
                    if (enemies.Contains(tiles[tmpIndex]) && !deadEnemies.Contains(tiles[tmpIndex]))
                    {
                        break;
                    }
                }
            }

            for(int k=0; k<2; k++)
            {
                for(int i=0; i<size; i++)
                {
                    int tmpCol;
                    if (k == 0)
                    {
                        // East
                        tmpCol = col + 1 + i;
                        if (tmpCol >= size)
                            break;
                    }
                    else
                    {
                        // West
                        tmpCol = col - 1 - i;
                        if (tmpCol < 0)
                            break;
                    }
                    // Get the index of the tile we are checking
                    int tmpIndex = MathUtility.MatrixCoordsToArrayIndex(row, tmpCol, size);
                    // If the tile is a tower and it's not dead you die
                    if ("tower".Equals(tiles[tmpIndex].name.ToLower()) && !deadEnemies.Contains(tiles[tmpIndex]))
                    {
                        return true;
                    }

                    // If tile is an enemy ( but not a tower ) and is not dead you get some cove
                    if (enemies.Contains(tiles[tmpIndex]) && !deadEnemies.Contains(tiles[tmpIndex]))
                    {
                        break;
                    }
                }
                
            }

            return false;
        }

        bool EatByPawnOrBishop(int row, int col)
        {
            // Check north east
            for(int k=0; k<4; k++)
            {
                for (int i = 0; i < size - 1; i++)
                {
                    int tmpRow, tmpCol;
                    if(k == 0) 
                    {
                        // North east
                        tmpRow = row - 1 - i;
                        tmpCol = col + 1 + i;
                        if (tmpRow < 0 || tmpCol >= size)
                            break;
                    }
                    else if (k == 1)
                    {
                        // South east
                        tmpRow = row + 1 + i;
                        tmpCol = col + 1 + i;
                        if (tmpRow >= size || tmpCol >= size)
                            break;
                    }
                    else if (k == 2)
                    {
                        // South west
                        tmpRow = row + 1 + i;
                        tmpCol = col - 1 - i;
                        if (tmpRow >= size || tmpCol < 0)
                            break;
                    }
                    else
                    {
                        // North west
                        tmpRow = row - 1 - i;
                        tmpCol = col - 1 - i;
                        if (tmpRow < 0 || tmpCol < 0)
                            break;
                    }

                    // Get the index of the tile we are checking
                    int tmpIndex = MathUtility.MatrixCoordsToArrayIndex(tmpRow, tmpCol, size);


                    if (i == 0 && (k==0 || k==3))// Check the first tile to the north ( pawn and bishop )
                    {
                        // If is a bishop or a pawn and is alive you die
                        bool bishop = "bishop".Equals(tiles[tmpIndex].name.ToLower());
                        bool pawn = "pawn".Equals(tiles[tmpIndex].name.ToLower());
                        if ((bishop || pawn) && !deadEnemies.Contains(tiles[tmpIndex]))
                            return true;

                    }
                    else // Check the other ( only bishop )
                    {
                        bool bishop = "bishop".Equals(tiles[tmpIndex].name.ToLower());
                        if (bishop && !deadEnemies.Contains(tiles[tmpIndex]))
                            return true;
                    }

                    // If is another enemy and is alive you get some cover
                    if (enemies.Contains(tiles[tmpIndex]) && !deadEnemies.Contains(tiles[tmpIndex]))
                        break;

                }
            }
                     

            return false;
        }
    }

}
