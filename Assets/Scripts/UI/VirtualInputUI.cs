using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.UI
{
    public class VirtualInputUI : MonoBehaviour
    {
        [SerializeField]
        GameObject leftJoystick;

        [SerializeField]
        GameObject rightJoystick;

        [SerializeField]
        GameObject actionButton;

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
            //leftJoystick.SetActive(false);
            //rightJoystick.SetActive(false);
            //actionButton.SetActive(false);
            gameObject.SetActive(false);
        }

        void HandleOnPuzzleExit(PuzzleController puzzleController)
        {
            //leftJoystick.SetActive(true);
            //rightJoystick.SetActive(true);
            //actionButton.SetActive(true);
            gameObject.SetActive(true);
        }
    }

}
