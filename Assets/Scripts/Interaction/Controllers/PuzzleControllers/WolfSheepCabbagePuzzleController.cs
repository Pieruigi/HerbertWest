using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Audio;
using Zom.Pie.Collections;

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

        [SerializeField]
        GameObject cover;

        [SerializeField]
        ItemPicker itemPicker;

        [SerializeField]
        ClipData handleClip;

        [SerializeField]
        ClipData handleResetClip;

        [SerializeField]
        ClipData lockResetClip;

        [SerializeField]
        ClipData lockOpenClip;

        [SerializeField]
        ClipData coverOpenClip;

        // 0 is back, 1 is forward.
        int[] handleValues;

        List<Vector3> defaultPositions;
        
        float disp = -0.125f;
        float moveTime = 0.25f;

        bool wait = false;
        int lastMoveId = -1;

        int errorMsgId = 4;

        AudioSource audioSource;

        protected override void Awake()
        {
            base.Awake();

            // Get the audio source
            audioSource = GetComponent<AudioSource>();

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
                

                // Open cover.
                cover.transform.position += cover.transform.right * 0.2f;

                Debug.Log("Completed");
                for (int i = 0; i < handles.Count; i++)
                {
                    handleValues[i] = 1;
                    LeanTween.moveLocalX(handles[i], disp, 0f);
                }

                // Set the picker as picked.
                itemPicker.SetSceneObjectAsPicked();


            }
        }

        public override void Exit()
        {
            wait = false;
            lastMoveId = -1;

            base.Exit();


        }

        public override void Interact(Interactor interactor)
        {
            if (wait)
                return;

            // Get the index of the handle we interacted to.
            int handleIndex = GetHandleIndex(interactor);

            StartCoroutine(DoInteraction(handleIndex));
        }

        

        int GetHandleIndex(Interactor interactor)
        {
            return handles.FindIndex(h => h.GetComponent<Interactor>() == interactor);
        }

        IEnumerator DoInteraction(int handleIndex)
        {
            OnPuzzleInteractionStart?.Invoke(this);

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

            // Move the handle
            LeanTween.move(handles[handleIndex], targetPosition, moveTime).setEaseInExpo();

            // Play audio
            handleClip.Play(audioSource);

            yield return new WaitForSeconds(moveTime);

            if (!CheckRules())
            {
                yield return new WaitForSeconds(0.5f);

                // Play clip
                lockResetClip.Play(audioSource);

                // Failed, reset all the handles.  
                yield return new WaitForSeconds(1f);
                for(int i=0; i<handles.Count; i++)
                {
                    handleValues[i] = 0;
                    targetPosition = defaultPositions[i];
                    LeanTween.move(handles[i], targetPosition, 3*moveTime).setEaseInOutExpo();
                }

                // Play clip
                handleResetClip.PlayDelayed(audioSource, 0.5f);

                // Wait for reset
                yield return new WaitForSeconds(3 * moveTime + 0.5f);

                // Send a message to the player.
                GetComponent<Messenger>().SendMessage((int)TextFactory.Type.InGameMessage, errorMsgId);
            }
            else
            {
                // Check if is completed.
                if (CheckCompleted())
                {
                    yield return new WaitForSeconds(0.5f);

                    // Play clip
                    lockOpenClip.PlayDelayed(audioSource, 0.5f);

                    yield return new WaitForSeconds(1f);

                    // Set completed.
                    SetStateCompleted();

                    // Open the box.
                    LeanTween.move(cover, cover.transform.position + cover.transform.right * 0.2f, 0.750f);

                    // Play clip
                    coverOpenClip.Play(audioSource);

                    // Wait until the box opens.
                    yield return new WaitForSeconds(1f);

                    // We are calling the picking method here rather than using the fsm change state
                    // because we want to wait for the picking to complete.
                    yield return itemPicker.Pick();

                    // Wait a bit.
                    yield return new WaitForSeconds(0.5f);

                    // Exit.
                    Exit();
                }
                    
            }

            wait = false;
            OnPuzzleInteractionStop?.Invoke(this);
        }

        bool CheckRules()
        {
            // We are moving the wolf but sheep and cabbage are already to the other side.
            if (lastMoveId == 0 && (handleValues[1] == handleValues[2]))
                return false;

            // We are moving the cabbage but sheep and wolf are already to the other side.
            if (lastMoveId == 2 && (handleValues[0] == handleValues[1]))
                return false;

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
