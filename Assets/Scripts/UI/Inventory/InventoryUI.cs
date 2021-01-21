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
        public UnityAction<Item> OnItemChosen;
        public UnityAction OnClosed;

        [SerializeField]
        GameObject panel;

        [SerializeField]
        Transform container;

        [SerializeField]
        GameObject itemTemplate;

        [SerializeField]
        Button buttonUse;

        [SerializeField]
        Text descriptionField;

        [SerializeField]
        Text wrongItemField;

        bool open = false;

        
        //bool useEnabled = false;

        Item selectedItem = null;

        string wrongItemMessage;

        public static InventoryUI Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

                // Set use button not interactable as default.
                buttonUse.interactable = false;

                wrongItemField.text = "";
                wrongItemMessage = TextFactory.Instance.GetText(TextFactory.Type.InGameMessage, 2); 

                SelectItem(null);

                // Deactivate the inventory panel.
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

        public void Open(bool useEnabled)
        {
            // If already open then return.
            if (open)
                return;

            PlayerManager.Instance.SetDisable(true);

            open = true;

            // If we are opening the inventory by clicking on some object that needs a given item then
            // we must set the useButton enable.
            if (useEnabled)
            {
                buttonUse.interactable = true;
            }
            
            panel.SetActive(true);

            // Fill content UI
            FillContent();
        }

        public void Close()
        {
            if (!open)
                return;

            ClearAll();

            // Reset use button.
            buttonUse.interactable = false;

            open = false;
            panel.SetActive(false);

            PlayerManager.Instance.SetDisable(false);

            OnClosed?.Invoke();

            
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

            wrongItemField.text = "";

            OnItemChosen?.Invoke(selectedItem);
        }

        public void ShowWrongItemMessage()
        {
            wrongItemField.text = wrongItemMessage;
        }

        private void FillContent()
        {
            // Create a new toggle for each item in the inventory.
            bool first = true;
            foreach(Item item in Inventory.Instance.Items)
            {
                // Create object from template.
                GameObject o = GameObject.Instantiate(itemTemplate, container, false);
                o.GetComponent<ItemUI>().Init(item);
                Toggle t = o.GetComponent<Toggle>();

                // Set the first item selected...
                if (first)
                {
                    first = false;
                    t.isOn = true;
                    SelectItem(item);
                }
                else //... and the others off.
                {
                    t.isOn = false;
                }

                // Set handle.
                t.onValueChanged.AddListener((Toggle) => HandleOnValueChanged(t, t.isOn));

                o.SetActive(true);
            }
        }

        private void ClearAll()
        {
            // Clear selected item.
            SelectItem(null);

            wrongItemField.text = "";
            
            // Remove all the ui items.
            int count = container.childCount;
            for(int i=0; i<count; i++)
            {
                DestroyImmediate(container.GetChild(0).gameObject);
            }
        }

        /// <summary>
        /// Manages toggle value on change.
        /// </summary>
        /// <param name="value"></param>
        private void HandleOnValueChanged(Toggle toggle, bool value)
        {
            // If value true then set the caller as the selected item.
            if (value)
            {
                SelectItem(toggle.GetComponent<ItemUI>().Item);
            }
        }

        private void SelectItem(Item item)
        {
            selectedItem = item;
            string desc = "";
            if(selectedItem != null)
            {
                desc = selectedItem.GetDescription();
            }
            descriptionField.text = desc;
        }

        
    }

}
