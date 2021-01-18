using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Collections
{
    public class Item : ScriptableObject
    {
       
        [SerializeField]
        string code;
        public string Code
        {
            get { return code; }
        }

        [SerializeField]
        string description;

        [SerializeField]
        Texture2D icon;
    }

}