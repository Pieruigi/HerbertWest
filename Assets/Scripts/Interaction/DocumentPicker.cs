using Aura2API;
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
        AuraLight  auraLight;

        // The internal finite state machine
        FiniteStateMachine fsm;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            // Destroy the light if the decument has been picked.
            if(fsm.CurrentStateId == PickedState)
            {
                Destroy(auraLight.gameObject);
            }
        }


        protected override object GetObject()
        {
            return document;
        }

        protected override IEnumerator PickEffect()
        {
            
            // Increase aura light strength.
            float time = 0.5f;
            float targetStrength = 5f;
            LeanTween.value(gameObject, OnStrengthUpdate, auraLight.strength, targetStrength, time).setEaseInOutBounce();

            yield return new WaitForSeconds(time);

            // Destroy the book.
            Destroy(SceneObject);

            // Decrease light strength.
            LeanTween.value(gameObject, OnStrengthUpdate, targetStrength, 0, time).setEaseInOutBounce();

            yield return new WaitForSeconds(time);

            Destroy(auraLight.gameObject);
            
        }

        void OnStrengthUpdate(float value)
        {
            auraLight.strength = value;
        }
    }

}
