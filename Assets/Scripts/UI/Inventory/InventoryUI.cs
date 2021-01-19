using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class InventoryUI : MonoBehaviour
    {
        public UnityAction<Item> OnItemUsed;

        [SerializeField]
        GameObject panel;

        [SerializeField]
        Transform container;

        [SerializeField]
        GameObject itemTemplate; 

        bool open = false;

        Item selectedItem = null;

        public static InventoryUI Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                panel.SetActive(false);

                // Move the template outside.
                itemTemplate.transform.parent = container.parent;
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

            // Fill content UI
            FillContent();

            // If the inventory is not empty the first item is the selected one.
            if (Inventory.Instance.Items.Count > 0)
                selectedItem = Inventory.Instance.Items[0];
        }

        public void Close()
        {
            if (!open)
                return;

            ClearAll();

            open = false;
            panel.SetActive(false);
        }

        public bool IsOpen()
        {
            return open;
        }

        /// <summary>
        /// Try to use the selected item.
        /// This method can be used for exaple if you want to use a key in order to open a locked door, or
        /// if you want to put something you have in your inventory in some devide ( to solve a puzzle for
        /// example ).
        /// </summary>
        public void UseSelectedItem()
        {
            if (selectedItem == null)
                return;

            OnItemUsed?.Invoke(selectedItem);
        }

        private void FillContent()
        {
            // Create a new toggle for each item in the inventory.
            foreach(Item item in Inventory.Instance.Items)
            {
                // Create object from template.
                GameObject o = GameObject.Instantiate(itemTemplate, container, false);
                o.GetComponent<ItemUI>().Init(item);
                o.SetActive(true);
            }
        }

        private void ClearAll()
        {
            // Remove all the ui items.
            int count = container.childCount;
            for(int i=0; i<count; i++)
            {
                Destroy(container.GetChild(0).gameObject);
            }
        }

        private void CheckSelected()
        {
            
        }
    }

}
