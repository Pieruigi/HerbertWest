using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class LibraryBookController : MonoBehaviour
    {
        public static readonly int PushedState = 0;
        public static readonly int NotPushedState = 1;

        GameObject bookObject;

        float disp = 0.1f;
        float zDefault;

        FiniteStateMachine fsm;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
        }

        // Start is called before the first frame update
        void Start()
        {
            bookObject = transform.GetChild(0).gameObject;
            zDefault = bookObject.transform.position.z;

            if (fsm.CurrentStateId == PushedState)
            {
                Vector3 pos = bookObject.transform.position;
                pos.z += disp;
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
            
            if(fsm.CurrentStateId == PushedState)
            {
                // Move forward
                LeanTween.moveZ(bookObject, zDefault + disp, 1.0f).setEaseInOutExpo();
            }
            else
            {
                // Move back
                LeanTween.moveZ(bookObject, zDefault, 1.0f).setEaseInOutExpo().setEaseOutBounce();
                StartCoroutine(ResetState());
            }
        }

        IEnumerator ResetState()
        {
            yield return new WaitForSeconds(1f);
            fsm.ForceState(NotPushedState, false, false);
        }

    }

}
