using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;

namespace Zom.Pie
{
    public class InventoryCacher : Cacher
    {
        protected override string GetValue()
        {
            // Read data from the inventory and build the cache value string.
            string ret = null;
            foreach(Item item in GetComponent<Inventory>().Items)
            {
                if (ret == null)
                {
                    ret = item.Code;
                }
                else
                {
                    ret += " " + item.Code;
                }
            }

            return ret;
        }

        protected override void Init(string value)
        {
            Debug.LogFormat("Cacher - code:{0}, value:{1}", Code, value);
            // Fill the inventory by reading data from cache.
            string[] codes = value.Split(' ');
            foreach(string code in codes)
            {
                // Get the corresponding resource.
                Item[] items = Resources.LoadAll<Item>(Constants.ItemResourceFolder);
                Item item = new List<Item>(items).Find(i => i.Code.ToLower().Equals(code.ToLower()));

                // Add the item to the inventory.
                GetComponent<Inventory>().Add(item);
            }
        }

    }

}
