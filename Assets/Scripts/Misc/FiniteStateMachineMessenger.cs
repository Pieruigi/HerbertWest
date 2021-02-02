using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;

namespace Zom.Pie
{
    public class FiniteStateMachineMessenger : Messenger
    {
        [SerializeField]
        float delay = 1f;

        FiniteStateMachine fsm;

        protected override void Awake()
        {
            base.Awake();
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
            fsm.OnFail += HandleOnFail;
        }
        
        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if(fsm.LastExitCode > 0)
            {
                StartCoroutine(SendMessageDelayed((int)TextFactory.Type.InGameMessage, fsm.LastExitCode));
            }
        }

        void HandleOnFail(FiniteStateMachine fsm)
        {
            if (fsm.LastExitCode > 0)
            {
                StartCoroutine(SendMessageDelayed((int)TextFactory.Type.InGameMessage, fsm.LastExitCode));
            }
        }

        IEnumerator SendMessageDelayed(int messageType, int messageIndex)
        {
            yield return new WaitForSeconds(delay);

            SendMessage(messageType, messageIndex);
        }

    }

}
