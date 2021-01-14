using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Collections
{
    public class TextCollection: ScriptableObject
    {
        [SerializeField]
        List<string> textList;

        public string GetText(int index)
        {
            return textList[index];
        }
    }

}
