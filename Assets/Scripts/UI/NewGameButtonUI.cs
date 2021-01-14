using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class NewGameButtonUI : MonoBehaviour
    {

        private void Awake()
        {
            // Add the listener to the button.
            GetComponent<Button>().onClick.AddListener(StartNewGame);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartNewGame()
        {
            if (GameManager.Instance.IsSaveGameAvailable())
            {
                // Show a message box to tell player that is deleting any save game.
                MessageBox.Show(MessageBox.Type.YesNo, "L'attuale salvataggio verrà eliminato.\nContinuare?", CallbackYes, CallbackNo);
            }
            else
            {
                GameManager.Instance.StartNewGame();
            }

        }

        /// <summary>
        /// Yes I want to start a new game and delete any save game.
        /// </summary>
        void CallbackYes()
        {
            GameManager.Instance.StartNewGame();
        }

        /// <summary>
        /// No I don't want to start a new game.
        /// </summary>
        void CallbackNo()
        {

        }
    }

}
