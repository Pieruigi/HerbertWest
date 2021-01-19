using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;

namespace Zom.Pie
{
    public class FiniteStateMachineMessenger : Messenger
    {
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
                SendMessage((int)TextFactory.Type.InGameMessage, fsm.LastExitCode);
            }
        }

        void HandleOnFail(FiniteStateMachine fsm)
        {
            if (fsm.LastExitCode > 0)
            {
                SendMessage((int)TextFactory.Type.InGameMessage, fsm.LastExitCode);
            }
        }

    }

}
