using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;

namespace Zom.Pie
{
    public class Inventory : MonoBehaviour
    {
        // The list of items.
        List<Item> items = new List<Item>();
        public IList<Item> Items
        {
            get { return items.AsReadOnly(); }
        }

        public static Inventory Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
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

        /// <summary>
        /// Adds an item to the inventory.
        /// </summary>
        /// <param name="item"></param>
        public void Add(Item item)
        {
            items.Add(item);
        }

        /// <summary>
        /// Removes an item from the inventory.
        /// </summary>
        /// <param name="item"></param>
        public void Remove(Item item)
        {
            items.Remove(item);
        }
    }

}
