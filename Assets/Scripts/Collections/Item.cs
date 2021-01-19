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
        Sprite icon;
        public Sprite Icon
        {
            get { return icon; }
        }

        public string GetDescription()
        {
            ItemInfo info = GetFileInfo();
            return info.Description;
        }

        ItemInfo GetFileInfo()
        {
            string fileName = name + "_info_" + GameManager.Instance.Language.ToString();
            return Resources.Load<ItemInfo>(Constants.ItemResourceFolder + "/" + fileName);
        }
    }

}
