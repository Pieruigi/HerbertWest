using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zom.Pie.Collections;
using Zom.Pie.UI;

namespace Zom.Pie
{
    public class InventoryUser
    {
        public UnityAction<Item> OnItemUsed;

        System.Action<Item> itemChosenCallback;

        MonoBehaviour monoBehaviour;

        public InventoryUser(MonoBehaviour monoBehaviour, System.Action<Item> itemChosenCallback)
        {
            this.monoBehaviour = monoBehaviour;
            this.itemChosenCallback = itemChosenCallback;
        }

        public void Open()
        {
             monoBehaviour.StartCoroutine(CoroutineOpenInventory());
        }

        public void ReportRightItem()
        {
            InventoryUI.Instance.Close();
        }

        public void ReportWrongItem()
        {
            InventoryUI.Instance.ShowWrongItemMessage();
        }

        IEnumerator CoroutineOpenInventory()
        {
            yield return new WaitForSeconds(0.5f);

            if (InventoryUI.Instance.IsOpen())
                yield break;

            // Set the handle.
            InventoryUI.Instance.OnItemChosen += HandleOnItemChosen;

            // Open the inventory.
            InventoryUI.Instance.Open(true);
        }

        void HandleOnItemChosen(Item item)
        {
            itemChosenCallback?.Invoke(item);
        }

        void HandleOnClosed()
        {
            InventoryUI.Instance.OnItemChosen -= HandleOnItemChosen;
        }
    }

}
