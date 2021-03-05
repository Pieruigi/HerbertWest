using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;

namespace Zom.Pie
{
    public class InteractionRaycast : Interactor
    {
        

        [SerializeField]
        [Tooltip("The object you want to interact with.")]
        InteractionController interactionController;

        [Tooltip("If null the interaction controller collider will be taken into account.")]
        [SerializeField]
        Collider interactionCollider;

        // The maximum distance allowed for the player to interact.
        [SerializeField]
        float distance = 2f;

        [SerializeField]
        Transform pivot;

        float sqrDistance;

        bool inside = false;

        // Used to keep trace of the interaction event.
        // 1: enter
        // 2: exit
        int triggerDir = -1;

        protected override void Awake()
        {
            base.Awake();

            // Set the current object as pivot if needed
            if (!pivot)
                pivot = transform;

            sqrDistance = distance * distance;

            if(!interactionCollider && interactionController)
            {
                interactionCollider = interactionController.GetComponent<Collider>();
            }    
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the player is too far we don't cast any ray.
            if (!inside)
            {
                if (triggerDir == 1)
                    CallTriggerEvent();
                return;
            }
                

            // Cast a ray from the camera.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo;
            PlayerManager.Instance.GetComponent<Collider>().enabled = false; // Avoid player collider
            bool hit = Physics.Raycast(ray, out hitInfo, 2 * distance);
            PlayerManager.Instance.GetComponent<Collider>().enabled = true;
            if (hit)
            {
                //Debug.LogFormat("Hit something: {0}", hitInfo.collider.gameObject);
                // Check if we hit the interaction controller collider.
                if(hitInfo.collider == interactionCollider)
                {
                    // We hit the right object, now try to interact with it.
                    if (interactionController.InteractionAllowed())
                    {
                        if (triggerDir == -1)
                            CallTriggerEvent();
                            

                        //Debug.Log("Is interaction allowed");
                        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                        {
                            interactionController.Interact();
                        }
                        
                    }
                    else
                    {
                        if (triggerDir == 1)
                            CallTriggerEvent();

                    }

                }
                else
                {
                    if (triggerDir == 1)
                        CallTriggerEvent();
                }
            }
            else
            {
                if (triggerDir == 1)
                    CallTriggerEvent();
            }
        }

        private void FixedUpdate()
        {
            // Check if the player is too far away.
            if ((PlayerManager.Instance.transform.position - pivot.position).sqrMagnitude < sqrDistance)
            {
                inside = true;
                //Debug.Log("Is inside...");
            }
            else
            {
                inside = false; 
                //Debug.Log("Is outside...");
            }
        }

        public override void SetEnable(bool value)
        {
            interactionCollider.enabled = value;
            enabled = value;
        }

        private void CallTriggerEvent()
        {
            triggerDir *= -1;
            if (triggerDir > 0)
                OnTriggerEnter?.Invoke(this);
            else
                OnTriggerExit?.Invoke(this);
        }
    }

}
