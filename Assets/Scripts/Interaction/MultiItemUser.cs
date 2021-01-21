using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;
using Zom.Pie.UI;

namespace Zom.Pie
{
    public class MultiItemUser : MonoBehaviour
    {
        [SerializeField]
        List<Item> targetItems;

        [SerializeField]
        [Tooltip("Each object corresponds to a given item in the targetItems.")]
        List<GameObject> sceneObjects;

        // Start is called before the first frame update
        void Start()
        {

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

        }
    }

}
