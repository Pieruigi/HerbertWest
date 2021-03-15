using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class DemoUI : MonoBehaviour
    {
        [SerializeField]
        GameObject panel;

        private void Awake()
        {
            panel.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Open()
        {
            

            panel.SetActive(true);
        }
    }

}
