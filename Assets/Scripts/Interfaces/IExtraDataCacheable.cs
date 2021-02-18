using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Zom.Pie.Interfaces
{
    public interface IExtraDataCacheable
    {
        string GetData();

        void Init(string extraData);
    }

}
