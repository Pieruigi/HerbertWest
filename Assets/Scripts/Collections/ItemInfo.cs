using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Collections
{
    /// <summary>
    /// Name format: Item_info ( ex. for the item Key this object must be Key_info ).
    /// </summary>
    public class ItemInfo : ScriptableObject
    {
        [SerializeField]
        string _name;
        public string Name
        {
            get { return name; }
        }

        [SerializeField]
        string description;
        public string Description
        {
            get { return description; }
        }
    }

}
