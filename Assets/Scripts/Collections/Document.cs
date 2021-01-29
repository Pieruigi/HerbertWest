using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Collections
{
    public class Document : ScriptableObject
    {
        [SerializeField]
        string code;
        public string Code
        {
            get { return code; }
        }

        
    }

}
