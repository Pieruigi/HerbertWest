using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCache : MonoBehaviour
{
    

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Zom.Pie.CacheManager.Instance.Save();
        }
    }
}
