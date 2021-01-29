using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.UI
{
    public class GameMenu : MonoBehaviour
    {
        MenuManager menuManager;

        private void Awake()
        {
            menuManager = GetComponent<MenuManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            // Hide on start
            menuManager.Close();
        }

        // Update is called once per frame
        void Update()
        {
            // Open and close
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (menuManager.IsOpen())
                    Close();
                else
                    Open();
            }
        }

        public void Close()
        {
            PlayerManager.Instance.SetDisable(false);
            menuManager.Close();
        }

        void Open()
        {
            PlayerManager.Instance.SetDisable(true);
            menuManager.Open();
        }
    }

}
