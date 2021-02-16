using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class LibraryBookController : MonoBehaviour
    {
        GameObject bookObject;

        float disp = 0.1f;

        FiniteStateMachine fsm;

        int pushedState = 0;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
        }

        // Start is called before the first frame update
        void Start()
        {
            bookObject = transform.GetChild(0).gameObject;

            if(fsm.CurrentStateId != pushedState)
            {
                Vector3 pos = bookObject.transform.position;
                pos.z -= disp;
                bookObject.transform.position = pos;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if (this.fsm != fsm)
                return;
            
            if(fsm.CurrentStateId == pushedState)
            {
                // Move forward
            }
            else
            {
                // Move back
            }
        }
    }

}
