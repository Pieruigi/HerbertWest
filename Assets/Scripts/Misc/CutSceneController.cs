using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Zom.Pie
{
    
    public class CutSceneController : MonoBehaviour
    {
        [SerializeField]
        PlayableDirector director;

        [SerializeField]
        bool playOnStart = false;

        FiniteStateMachine fsm;

        int completedState = 0;
        int readyState = 1;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
        }

        // Start is called before the first frame update
        void Start()
        {


            // Check the current state
            if(fsm.CurrentStateId == readyState)
            {
                if (playOnStart)
                {
                    fsm.ForceState(completedState, true, true);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if(fsm.CurrentStateId == completedState && fsm.PreviousStateId == readyState)
            {
                director.Play();
            }
        }
    }

}
