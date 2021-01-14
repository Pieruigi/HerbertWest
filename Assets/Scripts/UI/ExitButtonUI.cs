using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class ExitButtonUI : MonoBehaviour
    {

        private void Awake()
        {
            // Add the listener to the button.
            GetComponent<Button>().onClick.AddListener(Exit);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Exit()
        {
            // Show a message box to ask the player if he really want to exit.
            MessageBox.Show(MessageBox.Type.YesNo, "Sei sicuro di voler uscire dal gioco?", CallbackYes, CallbackNo);


        }

        /// <summary>
        /// Yes I want to exit.
        /// </summary>
        void CallbackYes()
        {
            GameManager.Instance.ExitGame();
        }

        /// <summary>
        /// No I don't want to exit.
        /// </summary>
        void CallbackNo()
        {

        }
    }

}
