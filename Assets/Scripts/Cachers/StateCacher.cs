using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class StateCacher : Cacher
    {
       
        protected override string GetValue()
        {
            return GetComponent<FiniteStateMachine>().CurrentStateId.ToString();
        }

        protected override void Init(string value)
        {
            GetComponent<FiniteStateMachine>().ForceState(int.Parse(value), false, false);
        }
    }

}
