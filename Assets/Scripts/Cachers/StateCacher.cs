using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class StateCacher : Cacher
    {
        public int state = 0;

        protected override string GetValue()
        {
            //return GetComponent<>
            return state.ToString();
        }

        protected override void Init(string value)
        {
            state = int.Parse(value);
        }
    }

}
