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
        int playingState = 1;
        int readyState = 2;


        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
            //director.played += delegate { Debug.Log("Playing..."); };
            director.stopped += delegate { 
                Debug.Log("Stopping..."); 
                fsm.ForceState(completedState, true, true); 
            };
        }

        // Start is called before the first frame update
        void Start()
        {


            // Check the current state
            if(fsm.CurrentStateId == readyState)
            {
                if (playOnStart)
                {
                    fsm.ForceState(playingState, true, true);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if(fsm.CurrentStateId == playingState && fsm.PreviousStateId == readyState)
            {
                director.Play();
            }
        }
    }

}
