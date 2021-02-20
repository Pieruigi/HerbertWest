using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Collections
{
    public class DocumentContent : ScriptableObject
    {
        [SerializeField]
        string description;
        public string Description
        {
            get { return description; }
        }

        [SerializeField]
        [TextArea(40, 100)]
        string body;
        public string Body
        {
            get { return body; }
        }
    }

}
