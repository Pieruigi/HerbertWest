using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class ContinueButtonUI : MonoBehaviour
    {
        
        // Start is called before the first frame update
        void Start()
        {
            // If there is no save game the continue button must be not interactable.
            if (!GameManager.Instance.IsSaveGameAvailable())
                GetComponent<Button>().interactable = false;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
