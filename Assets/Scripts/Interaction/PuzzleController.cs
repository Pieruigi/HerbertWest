using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Interfaces;

namespace Zom.Pie
{
    public class PuzzleController : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Dedicated camera.")]
        Camera puzzleCamera;

        [SerializeField]
        [Tooltip("All the objects we need to activate in order for the puzzle to work.")]
        List<GameObject> puzzleObjects;

        [SerializeField]
        [Tooltip("The fsm combination that solve the puzzle.")]
        int[] solution;

        int readyState = 0;

        bool opened = false;



        FiniteStateMachine fsm;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;

            // Disable interactors.
            ActivatePuzzleInteractors(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            // Set handles on puzzle objects.
            foreach(GameObject o in puzzleObjects)
            {
                FiniteStateMachine fsm = o.GetComponent<FiniteStateMachine>();
                if (fsm)
                {
                    // Set fsm handles.
                    fsm.OnStateChange += HandleOnStateChange;
                    fsm.OnFail += HandleOnFail;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void LateUpdate()
        {
            // To be sure the player remains disabled even if we open and close inventory we simply check.
            if (opened)
            {
                if (!PlayerManager.Instance.IsDisabled())
                    PlayerManager.Instance.SetDisable(true);
            }
        }

        protected virtual void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if(fsm == this.fsm) // Local fsm
            {
                if (fsm.CurrentStateId == readyState)
                    StartCoroutine(CoroutineEnter());
            }
            else // One of the other fsms
            {

            }
        }

        protected virtual void HandleOnFail(FiniteStateMachine fsm)
        {

        }

        IEnumerator CoroutineEnter()
        {
            opened = true;

            // Deactivate the owner.
            gameObject.SetActive(false);

            // Set camera.
            //Do some fade.

            // Activate all the objects.
            ActivatePuzzleInteractors(true);
            yield break;
        }

        IEnumerator CoroutineExit()
        {
            // Camera

            // Deactivate all the objects.
            ActivatePuzzleInteractors(false);

            opened = false;

            yield break;
        }

        void ActivatePuzzleInteractors(bool value)
        {
            foreach (GameObject o in puzzleObjects)
            {
                o.GetComponentInChildren<IInteractor>().Enable(value);

            }
        }
    }

    

}
