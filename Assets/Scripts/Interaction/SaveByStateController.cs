using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class SaveByStateController : MonoBehaviour
    {
        [System.Serializable]
        class Transition
        {
            public bool fromEveryState = false;
            public int fromState = -1;
            public int toState = -1;
        }

        [SerializeField]
        List<Transition> transitions;

        

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<FiniteStateMachine>().OnStateChange += HandleOnStateChange;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            Transition transition = transitions.Find(t => t.toState == fsm.CurrentStateId);

            if (transition == null)
                return;

            if (transition.fromEveryState == false && transition.fromState != fsm.PreviousStateId)
                return;

            StartCoroutine(SaveGame());
        }

        IEnumerator SaveGame()
        {
            yield return new WaitForEndOfFrame();
            CacheManager.Instance.Save();
        }
    }

}
