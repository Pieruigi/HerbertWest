using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.UI;
using Zom.Pie;
using Zom.Pie.Collections;

public class TestInventory : MonoBehaviour
{
    [SerializeField]
    List<Item> items;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Item item in items)
        {
            Inventory.Instance.Add(item);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    if(InventoryUI.Instance.IsOpen())
        //    {
        //        InventoryUI.Instance.Close();
        //    }
        //    else
        //    {
        //        InventoryUI.Instance.Open(true);
        //    }
        //}
    }
}
