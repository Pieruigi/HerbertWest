using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zom.Pie.Collections;

namespace Zom.Pie
{
    public class DocumentPicker : Picker
    {
        [SerializeField]
        Document document;

        //[SerializeField]
        //AuraLight  auraLight;

        // The internal finite state machine
        Bloom bloom;

        protected override void Awake()
        {
            base.Awake();

           
        }

        protected override void Start()
        {
            base.Start();

            // Destroy the light if the decument has been picked.
            if(FiniteStateMachine && FiniteStateMachine.CurrentStateId == PickedState)
            {
                //Destroy(auraLight.gameObject);
            }



            Volume volume = GameObject.FindObjectOfType<Volume>();
            bloom = null;
            volume.profile.TryGet(out bloom);
            
        }


        protected override object GetObject()
        {
            return document;
        }

        protected override IEnumerator PickEffect()
        {
            
            // Increase post processing bloom.
            float time = 1.5f;
            float targetIntensity = 12000f;
            float targetThreshold = 0f;

            float intensityDefault = 0, thresholdDefault = 0;

            if (bloom)
            {

                intensityDefault = bloom.intensity.value;
                thresholdDefault = bloom.threshold.value;
                Debug.Log("Bloom.strength:" + bloom.intensity);
                
                LeanTween.value(gameObject, OnIntensityUpdate, intensityDefault, targetIntensity, time);
                LeanTween.value(gameObject, OnThresholdUpdate, thresholdDefault, targetThreshold, time);
            }



            yield return new WaitForSeconds(time);

            // Destroy the book.
            
            time = 0.5f;
            LeanTween.value(gameObject, OnScaleUpdate, SceneObject.transform.localScale, Vector3.zero, time);

            

            // Decrease light strength.
            if (bloom)
            {
                LeanTween.value(gameObject, OnIntensityUpdate, targetIntensity, intensityDefault, time);
                LeanTween.value(gameObject, OnThresholdUpdate, targetThreshold, thresholdDefault, time);
            }

            yield return new WaitForSeconds(time);
            Destroy(SceneObject);

            //yield return new WaitForSeconds(time);

 
            
        }

        void OnIntensityUpdate(float value)
        {
            bloom.intensity.value = value;
        }


        void OnThresholdUpdate(float value)
        {
            bloom.threshold.value = value;
        }

        void OnScaleUpdate(Vector3 value)
        {
            SceneObject.transform.localScale = value;
        }
    }

}
