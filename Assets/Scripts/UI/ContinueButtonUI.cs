using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class ContinueButtonUI : MonoBehaviour
    {

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(HandleOnClick);
        }

        // Start is called before the first frame update
        void Start()
        {
            if (!GameManager.Instance.IsSaveGameAvailable())
            {
                GetComponent<Button>().interactable = false;
                Text text = GetComponentInChildren<Text>();
                Color c = text.color;
                c.a = 0.2f;
                text.color = c;
            }
                
            //if (!GameManager.Instance.InGame)
            //{
            //    // Player is not in game yet.
            //    // If there is no save game the continue button must be not interactable.
            //    if (!GameManager.Instance.IsSaveGameAvailable())
            //        GetComponent<Button>().interactable = false;
            //}
            //else
            //{
            //    // Player is in game.
            //    GetComponent<Button>().interactable = false;
            //}
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnClick()
        {
            GameManager.Instance.ContinueGame();

            //if (!GameManager.Instance.InGame)
            //{
            //    // If not in game then continue an old saved game...
            //    GameManager.Instance.ContinueGame();
            //}
            //else
            //{
            //    // ...otherwise just close this panel.
            //}
        }
    }

}
