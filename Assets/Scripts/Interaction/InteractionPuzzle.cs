using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Zom.Pie
{
    public class InteractionPuzzle : Interactor
    {
        [SerializeField]
        PuzzleController puzzleController;

        [SerializeField]
        Collider interactionCollider;

        bool enable = false;

        bool gamepadStyle = false;

        protected override void Awake()
        {
            base.Awake();
            if (!interactionCollider)
                interactionCollider = GetComponent<Collider>();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            if (!enable)
                return;

            if (!gamepadStyle)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //Debug.Log("Mouse button down.");
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if(Physics.Raycast(ray, out hit))
                    {
                        if(hit.collider == interactionCollider)
                        {
                            //Debug.Log("Hit collider:" + hit.collider);
                            puzzleController.Interact(this);
                        }
                    }
                }
            }
        }

        public override void Enable(bool value)
        {
            enable = value;

        }
    }

}
