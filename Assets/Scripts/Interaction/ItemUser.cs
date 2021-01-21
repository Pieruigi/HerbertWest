using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;
using Zom.Pie.UI;

namespace Zom.Pie
{
    public class ItemUser : MonoBehaviour
    {
        [SerializeField]
        Item targetItem;

        [SerializeField]
        bool keepInInventory = false;

        // The new state in case of success.
        [SerializeField]
        int newState = 0;

        FiniteStateMachine fsm;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
        }

        // Start is called before the first frame update
        void Start()
        {
            InventoryUI.Instance.OnClosed += HandleOnClosed;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CheckInventory()
        {
            StartCoroutine(CoroutineCheckInventory());
        }

        IEnumerator CoroutineCheckInventory()
        {
            yield return new WaitForSeconds(0.5f);

            if (InventoryUI.Instance.IsOpen())
                yield break;

            // Set the handle.
            InventoryUI.Instance.OnItemUsed += HandleOnItemUsed;

            // Open the inventory.
            InventoryUI.Instance.Open(true);
        }

        void HandleOnItemUsed(Item item)
        {
           

            // Try to use the item: on success we check if you need to remove it from the inventory and
            // set the fsm new state.
            if(item == targetItem)
            {
                // Close inventory.
                InventoryUI.Instance.Close();

                // Remove the item from the inventory if needed.
                if (!keepInInventory)
                {
                    Inventory.Instance.Remove(item);
                }

                // Set finite state machine new state.
                fsm.ForceState(newState, true, true);
            }
            else
            {
                // This is not the right item so we show an error message.
                InventoryUI.Instance.ShowWrongItemMessage();
            }
        }

        void HandleOnClosed()
        {
            InventoryUI.Instance.OnItemUsed -= HandleOnItemUsed;
        }
    }

}
