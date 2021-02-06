using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Zom.Pie
{
    public abstract class PuzzleController : MonoBehaviour
    {

        public UnityAction<PuzzleController> OnPuzzleEnterStart;
        public UnityAction<PuzzleController> OnPuzzleEnter;
        
        public UnityAction<PuzzleController> OnPuzzleExitStart;
        public UnityAction<PuzzleController> OnPuzzleExit;

        // Useful if you want to show/hide puzzle UI ( ex. button close ) while you are interacting.
        public UnityAction<PuzzleController> OnPuzzleInteractionStart;
        public UnityAction<PuzzleController> OnPuzzleInteractionStop;

        [SerializeField]
        Transform cameraTarget;

        [SerializeField]
        [Tooltip("The interactor of this object.")]
        GameObject interactor;


        [SerializeField]
        int readyState = 0;
        protected int ReadyState
        {
            get { return readyState; }
        }

        [SerializeField]
        int completedState = 1;
        protected int CompletedState
        {
            get { return completedState; }
        }

        [SerializeField]
        List<Light> lights;

        float cameraMoveTime = 0.5f;
        Vector3 cameraLastPosition;
        Vector3 cameraLastEulers;

        bool opened = false;
        public bool Open
        {
            get { return opened; }
        }
        bool busy = false;

        public bool Active
        {
            get { return opened && !busy; }
        }

        static List<PuzzleController> puzzleControllers;
        public static List<PuzzleController> PuzzleControllers
        {
            get { return puzzleControllers != null ? puzzleControllers : new List<PuzzleController>(); }
        }

        FiniteStateMachine fsm;
        protected FiniteStateMachine finiteStateMachine
        {
            get { return fsm; }
        }

        public abstract void Interact(Interactor interactor);

        #region native
        protected virtual void Awake()
        {
            // Add this object to the list
            if (puzzleControllers == null) 
                puzzleControllers = new List<PuzzleController>();
            puzzleControllers.Add(this);

            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;

            // Disable interactors.
            ActivatePuzzleInteractors(false);

            // Disable lights.
            EnableLights(false);
        }

        // Start is called before the first frame update
        protected virtual void Start(){}

        // Update is called once per frame
        protected virtual void Update(){}

        protected virtual void LateUpdate()
        {
            // To be sure the player remains disabled even if we open and close inventory we simply check.
            if (opened)
            {
                if (!PlayerManager.Instance.IsDisabled())
                    PlayerManager.Instance.SetDisable(true);
            }
        }

        #endregion
        public virtual void Exit()
        {
            StartCoroutine(CoroutineExit());
        }

        protected virtual void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if (fsm.CurrentStateId == readyState)
                StartCoroutine(CoroutineEnter());
            
        }

        protected virtual void HandleOnFail(FiniteStateMachine fsm){}

        protected virtual void SetStateCompleted()
        {
            fsm.ForceState(completedState, true, true);
        }

        IEnumerator CoroutineEnter()
        {
            OnPuzzleEnterStart?.Invoke(this);

            busy = true;
            opened = true;

            PlayerManager.Instance.SetDisable(true);

            // Enable lights.
            EnableLights(true);

            // Deactivate the local interactor
            interactor.GetComponent<Interactor>().Enable(false);

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

            // Disable lights.
            EnableLights(false);

            LeanTween.move(Camera.main.gameObject, cameraLastPosition, cameraMoveTime);
            LeanTween.rotate(Camera.main.gameObject, cameraLastEulers, cameraMoveTime);

            yield return new WaitForSeconds(cameraMoveTime);

            opened = false;

            // Activate the local interactor
            interactor.GetComponent<Interactor>().Enable(true);

            PlayerManager.Instance.SetDisable(false);

            busy = false;

            OnPuzzleExit?.Invoke(this);
        }

        void ActivatePuzzleInteractors(bool value)
        {
            Interactor[] interactors = GetComponentsInChildren<Interactor>();
            for(int i=0; i< interactors.Length; i++)
            {
                if (interactors[i] == interactor.GetComponent<Interactor>())
                    continue;

                interactors[i].Enable(value);
            }


        }

        void EnableLights(bool value)
        {
            foreach (Light l in lights)
            {
                l.enabled = value;
            }
        }
    }

    

}
