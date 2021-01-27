using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class VirtualInputUI : MonoBehaviour
    {


        PuzzleController puzzleController;
        Image[] images;

        // Start is called before the first frame update
        void Start()
        {
            foreach(PuzzleController pc in PuzzleController.PuzzleControllers)
            {
                pc.OnPuzzleEnterStart += HandleOnPuzzleEnterStart;
                pc.OnPuzzleExit += HandleOnPuzzleExit;
            }

            // Get all the images of the controller.
            images = GetComponentsInChildren<Image>();
        }


        void HandleOnPuzzleEnterStart(PuzzleController puzzleController)
        {
            foreach (Image i in images)
                i.enabled = false;
        }

        void HandleOnPuzzleExit(PuzzleController puzzleController)
        {
            foreach (Image i in images)
                i.enabled = true;
        }

    }

}
