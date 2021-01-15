using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class InteractionRaycast : MonoBehaviour
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

        float sqrDistance;

        bool inside = false;

        private void Awake()
        {
            sqrDistance = distance * distance;

            if(!interactionCollider)
            {
                interactionCollider = interactionController.GetComponent<Collider>();
            }    
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // If the player is too far we don't cast any ray.
            if (!inside)
                return;

            // Cast a ray from the camera.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray, out hitInfo, distance))
            {
                // Check if we hit the interaction controller collider.
                if(hitInfo.collider == interactionCollider)
                {
                    // We hit the right object, now try to interact with it.
                    if (interactionController.InteractionAllowed())
                        interactionController.Interact();
                }
            }
        }

        private void FixedUpdate()
        {
            // Check if the player is too far away.
            if ((PlayerManager.Instance.transform.position - transform.position).sqrMagnitude > sqrDistance * 1.5f)
            {
                inside = true;
            }
            else
            {
                inside = false;
            }
        }
    }

}
