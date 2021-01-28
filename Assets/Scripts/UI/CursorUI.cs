using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class CursorUI : MonoBehaviour
    {
        [SerializeField]
        Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
