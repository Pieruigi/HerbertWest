using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Interfaces;

namespace Zom.Pie
{
    public class StateCacher : Cacher
    {
       
        protected override string GetValue()
        {
            string ret = GetComponent<FiniteStateMachine>().CurrentStateId.ToString();
            if (GetComponent<IExtraDataCacheable>() != null)
            {
                ret += " " + GetComponent<IExtraDataCacheable>().GetData();
            }

            return ret;
        }

        protected override void Init(string value)
        {
            string[] splits = value.Trim().Split(' ');

            //GetComponent<FiniteStateMachine>().ForceState(int.Parse(value), false, false);
            GetComponent<FiniteStateMachine>().ForceState(int.Parse(splits[0]), false, false);
            if ( GetComponent<IExtraDataCacheable>() != null && splits.Length == 2)
            {
                GetComponent<IExtraDataCacheable>().Init(splits[1]);
            }
        }
    }

}
