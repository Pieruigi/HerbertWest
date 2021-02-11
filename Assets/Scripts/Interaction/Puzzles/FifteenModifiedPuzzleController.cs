using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class FifteenModifiedPuzzleController : PuzzleController
    {
        // Internal class useful to store row and column of each tile.
        class Tile
        {
            public GameObject tileObject;
            public int row, col;
        }

        [SerializeField]
        Transform tileContainer;

        [SerializeField]
        Transform targetContainer;


        List<Tile> tiles;
        List<Transform> targets;

        bool interacting = false;

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
                

            // Fill target positions
            targets = new List<Transform>();
            for (int i = 0; i < targetContainer.childCount; i++)
                targets.Add(targetContainer.GetChild(i));
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

            // Split interactor name according to its format
            string[] splits = interactor.name.Split('-');

            if("east".Equals(splits[0].ToLower()) || "west".Equals(splits[0].ToLower()))
            {
                // Try move row
                if(TryMoveRow("east".Equals(splits[0].ToLower()), int.Parse(splits[1])))
                {
                    Debug.Log("Moving east or west...");
                }
                else
                {
                    Debug.Log("Can't move");
                }
            }
            else // Is north or south
            {
                if (TryMoveColumn("north".Equals(splits[0].ToLower()), int.Parse(splits[1])))
                {
                    Debug.Log("Moving north or south...");
                }
                else
                {
                    Debug.Log("Can't move");
                }
            }

            yield return new WaitForSeconds(2f);



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
            return false;
        }

        /// <summary>
        /// Tries to move a column north or south ( depending on the first parameter ).
        /// </summary>
        /// <param name="east">true to move north, false to move south</param>
        /// <param name="index">the index of the column</param>
        /// <returns></returns>
        bool TryMoveColumn(bool north, int index)
        {
            return false;
        }
    }

}
