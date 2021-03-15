using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class VirtualInputUI : MonoBehaviour
    {
        public static VirtualInputUI Instance { get; private set; }

        PuzzleController puzzleController;
        Image[] images;
        Text[] texts;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

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
            texts = GetComponentsInChildren<Text>();
        }

        public void Show(bool value)
        {
            
            foreach (Image i in images)
            {
                //Color c = i.color;
                //c.a = value ? 1 : 0;
                i.enabled = value;
            }
                
            foreach (Text t in texts)
            {
                //Color c = t.color;
                //c.a = value ? 1 : 0;
                t.enabled = value;
            }
                
        }

        void HandleOnPuzzleEnterStart(PuzzleController puzzleController)
        {
            Show(false);
        }

        void HandleOnPuzzleExit(PuzzleController puzzleController)
        {
            Show(true);
        }

    }

}
