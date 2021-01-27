using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.UI;

public class TestPlayer : MonoBehaviour
{
    bool disabled = false;
    VirtualInputUI vi;
    // Start is called before the first frame update
    void Start()
    {
        vi = GameObject.FindObjectOfType<VirtualInputUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {

            //disabled = !disabled;
            //Zom.Pie.PlayerManager.Instance.SetDisable(disabled);
            
            Debug.Log("Vi:" + vi);
            if(vi.gameObject.activeSelf)
                vi.gameObject.SetActive(false);
            else
                vi.gameObject.SetActive(true);
        }
    }
}
