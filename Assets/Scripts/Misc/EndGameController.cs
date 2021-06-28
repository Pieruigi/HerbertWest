using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class EndGameController : MonoBehaviour
    { 
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Activate()
        {
            StartCoroutine(EndGameCoroutine());
        }

        IEnumerator EndGameCoroutine()
        {
            yield return new WaitForSeconds(5);

            GameManager.Instance.LoadMainMenu();
        }
    }

}
