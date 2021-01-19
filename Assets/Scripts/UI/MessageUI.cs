using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class MessageUI : MonoBehaviour
    {
        [SerializeField]
        GameObject panel;

        [SerializeField]
        Text textField;

        float timer = 0;
        float time = 5;

        private void Awake()
        {
            textField.text = "";
            panel.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            // Get all the messengers and set the handles.
            List<Messenger> messengers = new List<Messenger>(FindObjectsOfType<Messenger>());
            foreach(Messenger messenger in messengers)
            {
                messenger.OnMessageSent += HandleOnMessageSent;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;

                if(timer <= 0)
                {
                    panel.SetActive(false);
                    textField.text = "";
                }
            }
        }

        void HandleOnMessageSent(string message)
        {
            panel.SetActive(true);
            textField.text = message;
            timer = time;
        }
    }

}
