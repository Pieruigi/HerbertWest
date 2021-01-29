using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class PuzzleUI : MonoBehaviour
    {
        [SerializeField]
        Button exitButton;

        PuzzleController puzzleController;
        
        private void Awake()
        {
                
        }

        // Start is called before the first frame update
        void Start()
        {
      
            foreach(PuzzleController pc in PuzzleController.PuzzleControllers)
            {
                pc.OnPuzzleEnter += HandleOnEnter;
                pc.OnPuzzleExitStart += HandleOnExitStart;
                pc.OnPuzzleInteractionStart += HandleOnPuzzleInteractionStart;
                pc.OnPuzzleInteractionStop += HandleOnPuzzleInteractionStop;
            }

            gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Exit()
        {
            if (!puzzleController)
                return;

            puzzleController.Exit();
        }

        void HandleOnEnter(PuzzleController puzzleController)
        {
            this.puzzleController = puzzleController;
            gameObject.SetActive(true);
        }

        void HandleOnExitStart(PuzzleController puzzleController)
        {
            puzzleController = null;
            gameObject.SetActive(false);
            
        }

        void HandleOnPuzzleInteractionStart(PuzzleController puzzleController)
        {
            exitButton.interactable = false;
        }

        void HandleOnPuzzleInteractionStop(PuzzleController puzzleController)
        {
            exitButton.interactable = true;
        }
    }

}
