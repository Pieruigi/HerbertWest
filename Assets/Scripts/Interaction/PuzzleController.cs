using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zom.Pie.Interfaces;


namespace Zom.Pie
{
    public class PuzzleController : MonoBehaviour
    {
        public UnityAction<PuzzleController> OnPuzzleEnterStart;
        public UnityAction<PuzzleController> OnPuzzleEnter;
        public UnityAction<PuzzleController> OnPuzzleExitStart;
        public UnityAction<PuzzleController> OnPuzzleExit;


        [SerializeField]
        Transform cameraTarget;

        [SerializeField]
        [Tooltip("The interactor of this object.")]
        GameObject interactor;

        //[SerializeField]
        //[Tooltip("All the objects we need to activate in order for the puzzle to work.")]
        //List<GameObject> puzzleObjects;

        //[SerializeField]
        //[Tooltip("The fsm combination that solve the puzzle.")]
        //int[] solution;

        [SerializeField]
        int readyState = 0;

        [SerializeField]
        int completedState = 1;

        float cameraMoveTime = 0.5f;
        Vector3 cameraLastPosition;
        Vector3 cameraLastEulers;

        bool opened = false;

        static List<PuzzleController> puzzleControllers;
        public static List<PuzzleController> PuzzleControllers
        {
            get { return puzzleControllers; }
        }

        FiniteStateMachine fsm;

        bool busy = false;

        

        private void Awake()
        {
            // Add this object to the list
            if (puzzleControllers == null) puzzleControllers = new List<PuzzleController>();
            puzzleControllers.Add(this);

            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;

            // Disable interactors.
            ActivatePuzzleInteractors(false);
        }

        // Start is called before the first frame update
        void Start()
        {


            //// Set handles on puzzle objects.
            //foreach(GameObject o in puzzleObjects)
            //{
            //    FiniteStateMachine fsm = o.GetComponent<FiniteStateMachine>();
            //    if (fsm)
            //    {
            //        // Set fsm handles.
            //        fsm.OnStateChange += HandleOnStateChange;
            //        fsm.OnFail += HandleOnFail;
            //    }
            //}

        }

        // Update is called once per frame
        void Update()
        {
            //if (opened && !busy)
            //{
            //    if (Input.GetKeyDown(KeyCode.Escape))
            //        StartCoroutine(CoroutineExit());
            //}
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

        public void Exit()
        {
            StartCoroutine(CoroutineExit());
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
                // Check solution
                if (CheckSolution())
                {
                    fsm.ForceState(completedState, true, true);
                }
            }
        }

        protected virtual void HandleOnFail(FiniteStateMachine fsm)
        {

        }

        /// <summary>
        /// This should be an abstract class.
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckSolution()
        {
            return true;
        }

        IEnumerator CoroutineEnter()
        {
            OnPuzzleEnterStart?.Invoke(this);

            busy = true;
            opened = true;

            PlayerManager.Instance.SetDisable(true);

            // Deactivate the local interactor
            interactor.GetComponent<IInteractor>().Enable(false);

            // Set camera position and rotation.
            cameraLastPosition = Camera.main.transform.position;
            cameraLastEulers = Camera.main.transform.eulerAngles;
            LeanTween.move(Camera.main.gameObject, cameraTarget, cameraMoveTime);
            LeanTween.rotate(Camera.main.gameObject, cameraTarget.eulerAngles, cameraMoveTime);

            yield return new WaitForSeconds(cameraMoveTime);

            // Activate all the objects.
            ActivatePuzzleInteractors(true);
            busy = false;

            OnPuzzleEnter?.Invoke(this);
        }

        IEnumerator CoroutineExit()
        {
            OnPuzzleExitStart?.Invoke(this);

            busy = true;
            // Deactivate all the objects.
            ActivatePuzzleInteractors(false);

            LeanTween.move(Camera.main.gameObject, cameraLastPosition, cameraMoveTime);
            LeanTween.rotate(Camera.main.gameObject, cameraLastEulers, cameraMoveTime);

            yield return new WaitForSeconds(cameraMoveTime);

            opened = false;

            // Activate the local interactor
            interactor.GetComponent<IInteractor>().Enable(true);

            PlayerManager.Instance.SetDisable(false);

            busy = false;

            OnPuzzleExit?.Invoke(this);
        }

        void ActivatePuzzleInteractors(bool value)
        {
            IInteractor[] interactors = GetComponentsInChildren<IInteractor>();
            for(int i=0; i< interactors.Length; i++)
            {
                if (interactors[i] == interactor.GetComponent<IInteractor>())
                    continue;

                interactors[i].Enable(value);
            }

            //foreach (GameObject o in puzzleObjects)
            //{
            //    o.GetComponentInChildren<IInteractor>().Enable(value);

            //}
        }
    }

    

}
