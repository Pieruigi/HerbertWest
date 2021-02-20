using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zom.Pie.Collections;

namespace Zom.Pie.UI
{
    public class InGameReaderUI : MonoBehaviour
    {
        [SerializeField]
        Text textField;

        public static InGameReaderUI Instance { get; private set; }


        GameObject panel;

        bool enablePlayerOnClose = false;



        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                panel = transform.GetChild(0).gameObject;
                panel.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
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


        public void Open(Document document)
        {
            // Disable player controller
            if (!PlayerManager.Instance.IsDisabled())
            {
                enablePlayerOnClose = true;
                PlayerManager.Instance.SetDisable(true);
            }

            // Disable cursor
            CursorUI.Instance.Show(false);

            // Show the UI
            panel.SetActive(true);

            textField.text = document.GetBody();


            StartCoroutine(ResizeContent());
           
        }

        public void Close()
        {
            // Enable player
            if (enablePlayerOnClose)
                PlayerManager.Instance.SetDisable(false);

            // Enable cursor
            CursorUI.Instance.Show(true);

            panel.SetActive(false);
        }

        IEnumerator ResizeContent()
        {
            yield return new WaitForEndOfFrame();

            // Resize the content height
            RectTransform content = textField.transform.parent as RectTransform;
            Vector2 size = content.sizeDelta;
            size.y = (textField.transform as RectTransform).sizeDelta.y;
            Debug.Log("New size:" + size);
            content.sizeDelta = size;
        }
    }

}
