using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class VirtualInputHideImages : MonoBehaviour
    {
        bool hide = false;



        // Start is called before the first frame update
        void Start()
        {
            if (!hide)
                return;

            Image[] images = GetComponentsInChildren<Image>();
            foreach(Image image in images)
            {
                Color c = image.color;
                c.a = 0;
                image.color = c;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
