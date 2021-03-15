using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class Demo : MonoBehaviour
    {
        [SerializeField]
        DemoUI demoUI;

        bool inside = false;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (inside)
                return;


            if ("player".Equals(other.tag.ToLower()))
            {
                inside = true;
                GameManager.Instance.DisableAll = true;
                PlayerManager.Instance.SetDisable(true);
                demoUI.Open();
            }
        }
    }

}
