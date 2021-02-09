using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class PlayerCacher : Cacher
    {
        Vector3 position, eulerAngles;

        public Vector3 GetPosition()
        {
            return position;
        }

        public Vector3 GetEulerAngles()
        {
            return eulerAngles;
        }
       
        protected override string GetValue()
        {
            Transform t = GetComponent<PlayerManager>().transform;
            string ret = t.position.x.ToString();
            ret += " " + t.position.y.ToString();
            ret += " " + t.position.z.ToString();
            ret += " " + t.eulerAngles.x.ToString();
            ret += " " + t.eulerAngles.y.ToString();
            ret += " " + t.eulerAngles.z.ToString();

            return ret;
        }

        protected override void Init(string value)
        {
            Debug.LogFormat("'CacheValue:{0}'", value);
            string[] s = value.Split(' ');
            position = new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
            eulerAngles = new Vector3(float.Parse(s[3]), float.Parse(s[4]), float.Parse(s[5]));
            //PlayerManager pm = GetComponent<PlayerManager>();
            //pm.SetDisable(true);
            //pm.transform.position = new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
            //pm.transform.eulerAngles = new Vector3(float.Parse(s[3]), float.Parse(s[4]), float.Parse(s[5]));
            //pm.SetDisable(false);
        }
    }

}
