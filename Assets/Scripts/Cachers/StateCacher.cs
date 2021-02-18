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
            if (GetComponent<ICacheable>() != null)
            {
                ret += " " + GetComponent<ICacheable>().GetData();
            }

            return ret;
        }

        protected override void Init(string value)
        {
            //string[] splits = value.Trim().Split(' ');
            string state = null;
            string extra = null;
            if(value != null && value.Trim().Contains(" "))
            {
                state = value.Substring(0, value.IndexOf(' ')).Trim();
                extra = value.Substring(value.IndexOf(' ') + 1).Trim();
            }
            else
            {
                state = value;
            }
            
            

            //GetComponent<FiniteStateMachine>().ForceState(int.Parse(value), false, false);
            if(state != null)
                GetComponent<FiniteStateMachine>().ForceState(int.Parse(state), false, false);

            if ( GetComponent<ICacheable>() != null && extra != null)
                GetComponent<ICacheable>().Init(extra);
           
        }
    }

}
