using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class DiaryBlock : MonoBehaviour
    {
        [SerializeField]
        Collider blockCollider;

        FiniteStateMachine fsm;

        int freeState = 0;
        
        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (fsm.CurrentStateId == freeState)
                blockCollider.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if (fsm.CurrentStateId == freeState)
                blockCollider.enabled = false;
        }
    }

}
