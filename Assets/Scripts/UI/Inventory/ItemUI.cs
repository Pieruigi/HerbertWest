using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zom.Pie.Collections;

namespace Zom.Pie.UI
{
    public class ItemUI : MonoBehaviour
    {
        [SerializeField]
        Image image;

        Item item;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(Item item)
        {
            this.item = item;
            image.sprite = item.Icon;
        }
    }

}
