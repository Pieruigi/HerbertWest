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

        [SerializeField]
        int materialIndex;

        Material material;
        Color startColor;

        protected override void Awake()
        {
            base.Awake();
            material = SceneObject.GetComponent<MeshRenderer>().materials[materialIndex];

            
        }

        protected override object GetObject()
        {
            return document;
        }

        protected override IEnumerator PickEffect()
        {
            // Get starting color.
            startColor = material.GetColor("_EmissionColor");

            // Emission effect.
            float time = 1.5f;
            float targetPower = 100;
            LeanTween.value(gameObject, OnEmissionPowerUpdate, 1f, targetPower, time);

            yield return new WaitForSeconds(time);

            // Scale down.
            float scaleTime = 1;
            LeanTween.scale(SceneObject, Vector3.zero, scaleTime).setEaseInOutBounce();


            LeanTween.value(gameObject, OnEmissionPowerUpdate, targetPower, 0, scaleTime).setEaseInOutBounce();

            yield return new WaitForSeconds(scaleTime);
            
        }

        void OnEmissionPowerUpdate(float value)
        {
            material.SetColor("_EmissionColor", startColor * value);
        }
    }

}
