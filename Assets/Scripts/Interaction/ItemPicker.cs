using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;

namespace Zom.Pie
{
    public class ItemPicker : Picker
    {
        [SerializeField]
        Item item;


        [SerializeField]
        float distance;

        float time = 1f;

        protected override object GetObject()
        {
            return item;
        }

        protected override IEnumerator PickEffect(GameObject sceneObject)
        {
           
            LeanTween.move(sceneObject, Camera.main.transform.position + Camera.main.transform.forward * distance, time);

            yield return new WaitForSeconds(time+2f);

        }


    }

}
