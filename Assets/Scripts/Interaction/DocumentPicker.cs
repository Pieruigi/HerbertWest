using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zom.Pie.Collections;

namespace Zom.Pie
{
    public class DocumentPicker : InGameReaderController
    {
        [SerializeField]
        GameObject book;

        // The internal finite state machine
        Bloom bloom;

        int firstTimeState = 1;

        int unreabableState = 2;

        int materialId = 2;
        Material[] mats;

        protected override void Awake()
        {
            base.Awake();

            FiniteStateMachine.OnStateChange += HandleOnStateChange;

            mats = book.GetComponent<MeshRenderer>().materials;
            mats[materialId] = new Material(mats[materialId]);
        }

        protected override void Start()
        {
            base.Start();

            // Destroy the light if the decument has been picked.
            if(FiniteStateMachine && FiniteStateMachine.CurrentStateId == unreabableState)
            {
                // Set graphycs as unreadable
            }
            else
            {
                if (FiniteStateMachine && FiniteStateMachine.CurrentStateId == firstTimeState)
                {
                    // Set graphycs as unreadable
                }
                else
                {
                    // We read at least once
                    // Switch color 
                    mats[materialId].SetColor("_EmissionColor", Color.black);
                    book.GetComponent<MeshRenderer>().materials = mats;
                }
            }
            

            Volume volume = GameObject.FindObjectOfType<Volume>();
            bloom = null;
            volume.profile.TryGet(out bloom);
            
        }


        protected IEnumerator PickEffect()
        {
            PlayerManager.Instance.SetDisable(true);

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

            // Switch color 
            mats[materialId].SetColor("_EmissionColor", Color.black);
            book.GetComponent<MeshRenderer>().materials = mats;
            // Decrease light strength.
            time = 0.5f;
            if (bloom)
            {
                LeanTween.value(gameObject, OnIntensityUpdate, targetIntensity, intensityDefault, time);
                LeanTween.value(gameObject, OnThresholdUpdate, targetThreshold, thresholdDefault, time);
            }

            yield return new WaitForSeconds(time);

            PlayerManager.Instance.SetDisable(false);

            Zom.Pie.UI.InGameReaderUI.Instance.Open(Document);

        }

        void OnIntensityUpdate(float value)
        {
            bloom.intensity.value = value;
        }


        void OnThresholdUpdate(float value)
        {
            bloom.threshold.value = value;
        }



        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if(FiniteStateMachine.CurrentStateId == 0 && FiniteStateMachine.PreviousStateId == firstTimeState)
            {
                // Play effect and read
                StartCoroutine(PickEffect());
            }
        }

        protected override void HandleOnFail(FiniteStateMachine fsm)
        {
            //base.HandleOnFail(fsm);
            if (FiniteStateMachine.CurrentStateId == unreabableState)
                return;

            OpenUI();
        }
    }

}
