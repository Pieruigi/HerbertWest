using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Interfaces;

namespace Zom.Pie
{
    public class WolfSheepCabbagePuzzleController : PuzzleController
    {
        /// <summary>
        /// 0: wolf
        /// 1: sheep
        /// 2: cabbage
        /// </summary>
        [SerializeField]
        List<GameObject> handles;



        // 0 is back, 1 is forward.
        int[] handleValues;

        List<Vector3> defaultPositions;
        
        float disp = 0.1f;
        float moveTime = 0.5f;

        bool wait = false;
        int lastMoveId = -1;
        
        protected override void Awake()
        {
            base.Awake();

            defaultPositions = new List<Vector3>();
            foreach (GameObject handle in handles)
                defaultPositions.Add(handle.transform.position);

            handleValues = new int[handles.Count];
            
        }

        protected override void Start()
        {
            base.Start();

            if(finiteStateMachine.CurrentStateId == CompletedState)
            {
                Debug.Log("Completed");
                for(int i=0; i<handles.Count; i++)
                {
                    handleValues[i] = 1;
                    LeanTween.move(handles[i], defaultPositions[i] + handles[i].transform.right * disp, 0f);
                }
                
            }
        }

        public override void Exit()
        {
            wait = false;
            lastMoveId = -1;

            base.Exit();


        }

        public override void Interact(IInteractor interactor)
        {
            if (wait)
                return;

            // Get the index of the handle we interacted to.
            int handleIndex = GetHandleIndex(interactor);

            StartCoroutine(DoInteraction(handleIndex));
        }

        

        int GetHandleIndex(IInteractor interactor)
        {
            return handles.FindIndex(h => h.GetComponent<IInteractor>() == interactor);
        }

        IEnumerator DoInteraction(int handleIndex)
        {
            Debug.Log("HandleIndex:" + handleIndex);
            wait = true;
            lastMoveId = handleIndex;

            Vector3 targetPosition = defaultPositions[handleIndex];

            // If current value is 0 then we must move the handle forward
            if(handleValues[handleIndex] == 0)
            {
                handleValues[handleIndex] = 1;
                targetPosition += handles[handleIndex].transform.right * disp;
            }
            else
            {
                handleValues[handleIndex] = 0;
            }


            LeanTween.move(handles[handleIndex], targetPosition, moveTime);

            yield return new WaitForSeconds(moveTime);

            if (!CheckRules())
            {
                // Failed, reset.   
                yield return new WaitForSeconds(1f);
                for(int i=0; i<handles.Count; i++)
                {
                    handleValues[i] = 0;
                    targetPosition = defaultPositions[i];
                    LeanTween.move(handles[i], targetPosition, moveTime);
                }
            }
            else
            {
                // Check if is completed.
                if (CheckCompleted())
                {
                    SetStateCompleted();

                    yield return new WaitForSeconds(1f);
                    Exit();
                }
                    
            }

            wait = false;

        }

        bool CheckRules()
        {
            // If wolf, sheep and cabbage are togheter return true.
            if (handleValues[0] == handleValues[1] && handleValues[1] == handleValues[2])
                return true;

            // If wolf is with cabbage return true.
            if (handleValues[0] == handleValues[2])
                return true;

            // If wolf is with sheep but we just moved one of them return true.
            if ((handleValues[0] == handleValues[1]) && (lastMoveId == 0 || lastMoveId == 1))
                return true;

            // If cabbage is with sheep but we just moved one of them return true.
            if ((handleValues[1] == handleValues[2]) && (lastMoveId == 2 || lastMoveId == 1))
                return true;

            return false;
        }

        bool CheckCompleted()
        {
            foreach (int v in handleValues)
                if (v == 0)
                    return false;

            return true;
        }
    }

}
