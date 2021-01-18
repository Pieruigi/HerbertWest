using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField]
        GameObject panel;

        bool open = false;

        public static InventoryUI Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
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

        public void Open()
        {
            // If already open then return.
            if (open)
                return;

            open = true;
            panel.SetActive(true);
        }

        public void Close()
        {
            if (!open)
                return;

            open = false;
            panel.SetActive(false);
        }

        public bool IsOpen()
        {
            return open;
        }
    }

}
