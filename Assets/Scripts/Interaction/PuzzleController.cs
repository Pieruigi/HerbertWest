using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Interfaces;


namespace Zom.Pie
{
    public class PuzzleController : MonoBehaviour
    {

        [SerializeField]
        Transform cameraTarget;

        [SerializeField]
        [Tooltip("The interactor of this object.")]
        GameObject interactor;

        [SerializeField]
        [Tooltip("All the objects we need to activate in order for the puzzle to work.")]
        List<GameObject> puzzleObjects;

        [SerializeField]
        [Tooltip("The fsm combination that solve the puzzle.")]
        int[] solution;

        [SerializeField]
        int readyState = 0;

        [SerializeField]
        int completedState = 1;

        float cameraMoveTime = 0.5f;
        Vector3 cameraLastPosition;
        Vector3 cameraLastEulers;

        bool opened = false;

        public static List<PuzzleController> puzzleControllers = new List<PuzzleController>();

        FiniteStateMachine fsm;

        bool busy = false;

        private void Awake()
        {
            // Add this object to the list
            puzzleControllers.Add(this);

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
            if (opened && !busy)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    StartCoroutine(CoroutineExit());
            }
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
        }

        IEnumerator CoroutineExit()
        {
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
