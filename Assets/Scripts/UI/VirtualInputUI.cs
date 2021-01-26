using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.UI
{
    public class VirtualInputUI : MonoBehaviour
    {


        PuzzleController puzzleController;

        // Start is called before the first frame update
        void Start()
        {
            foreach(PuzzleController pc in PuzzleController.PuzzleControllers)
            {
                pc.OnPuzzleEnterStart += HandleOnPuzzleEnterStart;
                pc.OnPuzzleExit += HandleOnPuzzleExit;
            }
        }


        void HandleOnPuzzleEnterStart(PuzzleController puzzleController)
        {
            gameObject.SetActive(false);
        }

        void HandleOnPuzzleExit(PuzzleController puzzleController)
        {
            gameObject.SetActive(true);
        }
    }

}
