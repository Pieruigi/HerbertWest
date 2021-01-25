using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Interfaces;

namespace Zom.Pie
{
    public class InteractionPuzzle : MonoBehaviour, IInteractor
    {
        [SerializeField]
        PuzzleController puzzleController;

        [SerializeField]
        Collider interactionCollider;

        bool enable = false;

        bool gamepadStyle = false;

        void Awake()
        {
            if (!interactionCollider)
                interactionCollider = GetComponent<Collider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!enable)
                return;

            if (!gamepadStyle)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Mouse button down.");
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if(Physics.Raycast(ray, out hit))
                    {
                        if(hit.collider == interactionCollider)
                        {
                            Debug.Log("Hit collider:" + hit.collider);
                            puzzleController.Interact(this);
                        }
                    }
                }
            }
        }

        public void Enable(bool value)
        {
            enable = value;

        }
    }

}
