using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;
using Zom.Pie.Interfaces;
using Zom.Pie.UI;

namespace Zom.Pie
{
    public class ItemPlacer : MonoBehaviour, ICacheable
    {
        [SerializeField]
        List<GameObject> sceneObjects;

        [SerializeField]
        List<Item> items;

        bool[] missings;
        FiniteStateMachine fsm;

        int missingState = 1;
        int completedState = 0;

        void Awake()
        {
            if(!fsm)
                fsm = GetComponent<FiniteStateMachine>();

            fsm.OnStateChange += HandleOnStateChange;

            
        }

        // Start is called before the first frame update
        void Start()
        {
            
            // No cache yet
            if (missings == null)
            {
                missings = new bool[items.Count];
                for (int i = 0; i < missings.Length; i++)
                {
                    missings[i] = true;
                }
            }
            
            // Update objects
            for(int i=0; i<missings.Length; i++)
            {
                sceneObjects[i].SetActive(!missings[i]);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public string GetData()
        {
            // No data to send
            if (fsm.CurrentStateId == completedState)
                return "";

            // Send data
            string ret = "";
            for(int i=0; i<missings.Length; i++)
            {
                if (i == 0)
                    ret = missings[i] ? 1.ToString() : 0.ToString();
                else
                    ret += " " + (missings[i] ? 1.ToString() : 0.ToString());
            }
            return ret;
        }

        public void Init(string extraData)
        {
            if (!fsm)
                fsm = GetComponent<FiniteStateMachine>();

            missings = new bool[items.Count];
            if (fsm.CurrentStateId == completedState)
            {
                for(int i=0; i< missings.Length; i++)
                {
                    missings[i] = false;
                }
            }
            else
            {
                // Check for missing objects
                string[] splits = extraData.Split(' ');

                for (int i = 0; i < missings.Length; i++)
                {
                    missings[i] = "1".Equals(splits[i]) ? true : false;

                }
            }
            
        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            // Shoudn't really happen.
            if (this.fsm != fsm)
                return;

            if (fsm.CurrentStateId == completedState)
                return;

            // Open inventory
            InventoryUI.Instance.OnItemChosen += HandleOnItemChosen;
            InventoryUI.Instance.OnClosed += HandleOnInventoryClose;
            GameManager.Instance.OpenInventory(true);
        }

        void HandleOnItemChosen(Item item)
        {
            if (items.Contains(item))
            {
                // Close inventory UI
                GameManager.Instance.CloseInventory();

                // Remove from the inventory
                Inventory.Instance.Remove(item);

                // Update missings
                // Get the item id
                int id = items.IndexOf(item);
                // Set the missing field
                missings[id] = false;
                // Update in scene object
                sceneObjects[id].SetActive(true);

                // Check if we put all the items
                if (Completed())
                    fsm.ForceState(completedState, true, true);
            }
            else
            {
                // Wrong item
                InventoryUI.Instance.ShowWrongItemMessage();
            }
        }

        void HandleOnInventoryClose()
        {
            InventoryUI.Instance.OnClosed -= HandleOnInventoryClose;
            InventoryUI.Instance.OnItemChosen -= HandleOnItemChosen;
        }

        bool Completed()
        {
            for(int i=0; i<missings.Length; i++)
            {
                if (missings[i])
                    return false;
            }

            return true;
        }
    }

}
