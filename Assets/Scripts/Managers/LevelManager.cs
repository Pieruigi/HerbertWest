using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class LevelManager : MonoBehaviour
    {
        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Test());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Test()
        {
            Debug.Log("AAAAAAAAAAAAA");
            yield break;
        }

    }

}
