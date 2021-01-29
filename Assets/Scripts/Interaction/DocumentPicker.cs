using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;

namespace Zom.Pie
{
    public class DocumentPicker : Picker
    {
        [SerializeField]
        Document document;

        protected override object GetObject()
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerator PickEffect(GameObject sceneObject)
        {
            throw new System.NotImplementedException();
        }
    }

}
