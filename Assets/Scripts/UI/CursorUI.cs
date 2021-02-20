using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class CursorUI : MonoBehaviour
    {
        public static CursorUI Instance { get; private set; }

        [SerializeField]
        Image image;

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
            foreach (PuzzleController pc in PuzzleController.PuzzleControllers)
            {
                pc.OnPuzzleEnterStart += HandleOnEnterStart;
                pc.OnPuzzleExit += HandleOnExit;
                
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Show(bool value)
        {
            image.enabled = value;
        }

        public bool IsHidden()
        {
            return !image.enabled;
        }
        
        void HandleOnEnterStart(PuzzleController puzzleController)
        {
            image.enabled = false;
        }

        void HandleOnExit(PuzzleController puzzleController)
        {
            image.enabled = true;

        }

        
    }

}
