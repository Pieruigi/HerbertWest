using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestPlayer : MonoBehaviour
{
    bool disabled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            disabled = !disabled;
            Zom.Pie.PlayerManager.Instance.SetDisable(disabled);
        }
    }
}
