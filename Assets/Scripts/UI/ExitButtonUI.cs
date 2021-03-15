using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zom.Pie.Collections;

namespace Zom.Pie.UI
{
    public class ExitButtonUI : MonoBehaviour
    {
        [SerializeField]
        bool exitDemoButton = false;

        private void Awake()
        {
            if (!exitDemoButton)
            {
                // Add the listener to the button.
                GetComponent<Button>().onClick.AddListener(Exit);
            }
            
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
    
                 // Get message to show.
            int id = 1;
            if (!GameManager.Instance.InGame)
                id = 2;

            //string text = UIMessageFactory.Instance.GetText(id);
            string text = TextFactory.Instance.GetText(TextFactory.Type.UIMessage, id);

            // Show a message box to ask the player if he really want to exit.
            MessageBox.Show(MessageBox.Type.YesNo, text, CallbackYes, CallbackNo);
          
        }

        public void ExitDemo()
        {
            GameManager.Instance.ExitGame();
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
