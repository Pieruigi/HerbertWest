using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class ClockPuzzleController : PuzzleController
    {

        [SerializeField]
        Interactor hoursHandle;

        [SerializeField]
        Interactor minutesHandle;

        [SerializeField]
        Transform door;

        [SerializeField]
        Picker picker;

        bool interacting = false;

        int h = 1, m = 9;

        int hSolved = 7, mSolved = 6;

        float angle = 30;

        protected override void Awake()
        {
            base.Awake();

            hoursHandle.transform.localEulerAngles = Vector3.forward * angle * h;
            minutesHandle.transform.localEulerAngles = Vector3.forward * angle * m;
        }

        protected override void Start()
        {
            base.Start();

            if(finiteStateMachine.CurrentStateId == CompletedState)
            {
                hoursHandle.transform.localEulerAngles = Vector3.forward * angle * hSolved; 
                minutesHandle.transform.localEulerAngles = Vector3.forward * angle *mSolved;

                door.localEulerAngles = Vector3.up * 90;
                picker.SetSceneObjectAsPicked();
            }    
        }

        public override void Interact(Interactor interactor)
        {
            if (interacting)
                return;

            interacting = true;

            StartCoroutine(DoInteraction(interactor));
        }

        IEnumerator DoInteraction(Interactor interactor)
        {
            OnPuzzleInteractionStart?.Invoke(this);

            // Rotate handle
            if(interactor == hoursHandle)
            {
                h++;
                if (h == 12)
                    h = 0;
            }
            else
            {
                m++;
                if (m == 12)
                    m = 0;
            }
            float time = 0.5f;
            LeanTween.rotateAroundLocal(interactor.gameObject, Vector3.forward, angle, time).setEaseOutBack();
            yield return new WaitForSeconds(time);

            if (IsSolved())
            {
                // Open and pick
                LeanTween.rotateAroundLocal(door.gameObject, Vector3.up, 90, time).setEaseInOutBack();
                yield return new WaitForSeconds(time + 1f);

                yield return picker.Pick();
                yield return new WaitForSeconds(1f);

                SetStateCompleted();
                Exit();
            }

            interacting = false;
            OnPuzzleInteractionStop?.Invoke(this);
        }

        bool IsSolved()
        {

            return h == hSolved && m == mSolved;
        }
    }

}
