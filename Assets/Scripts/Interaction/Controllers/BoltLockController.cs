using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class BoltLockController : MonoBehaviour
    {
        [SerializeField]
        GameObject bolt;

        FiniteStateMachine fsm;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
        }

        // Start is called before the first frame update
        void Start()
        {
            if(fsm.CurrentStateId == 0)
            {
                LeanTween.moveLocalZ(bolt, 0, 0);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if(fsm.CurrentStateId == 0)
            {
                StartCoroutine(MoveBolt());
            }
        }

        IEnumerator MoveBolt()
        {
            
            float time = 1f;
            LeanTween.moveLocalX(bolt, 0, time);
            yield return new WaitForSeconds(time);

            
        }
    }

}
