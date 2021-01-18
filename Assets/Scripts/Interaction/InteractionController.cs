using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Zom.Pie
{
    public class InteractionController : MonoBehaviour
    {
        
        // We may want the object inactive in certain states.
        [SerializeField]
        List<int> unavailableStates = new List<int>();

        FiniteStateMachine fsm;
               
        // The last time we interact with the object.
        DateTime lastInteractionTime;

        // We may want to completely disable the object.
        bool disabled = false;

        float defaultCooldown = 0.5f;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
        }

       
        /// <summary>
        /// Avoid the object to be interacted.
        /// </summary>
        /// <param name="value"></param>
        public void ForceDisabled(bool value)
        {
            disabled = value;
        }

        /// <summary>
        /// If the object is available then FiniteStateMachine.LookUp() is called.
        /// </summary>
        public void Interact()
        {
            Debug.Log("Interact...");

            if (!InteractionAllowed())
                return;

            lastInteractionTime = DateTime.UtcNow;

            fsm.Lookup();
        }

        /// <summary>
        /// Returns true if if possible to interact with this object, otherwise returns false.
        /// </summary>
        /// <returns></returns>
        public bool InteractionAllowed()
        {
            if (disabled)
                return false;

            if (PlayerManager.Instance.IsDisabled())
                return false;

            // Wait
            if ((DateTime.UtcNow - lastInteractionTime).TotalSeconds < defaultCooldown)
                return false;

            // Not available for interaction.
            if (fsm.CurrentStateId == -1 || unavailableStates.Contains(fsm.CurrentStateId))
                return false;

            return true;
        }

    }

}
