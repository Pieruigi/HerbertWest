using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class GlobePuzzleController : PuzzleController
    {
        [SerializeField]
        GameObject globe;

        [SerializeField]
        GameObject leftArrow;

        [SerializeField]
        GameObject rightArrow;

        [SerializeField]
        GameObject boxCover;

        [SerializeField]
        Picker picker;

        //[SerializeField]
        float[] angles = new float[] { 0f, 14.7f, 33.1f, 51.6f, 70f, 103f, 
                                       127.5f, 166.5f, 208f, 245f, 263f, 287.1f, 305.8f,
                                       322.8f, 340f };

        int currentAngleId = 0;
        int lastAngleId = 0;

        bool interacting = false;

        int lastDir = 0;
        int step = 0;

        int[] solution = new int[] { 1, 11, 13, 2, 6 };

        bool openBox = false;
        float openAngle = -60f;


        protected override void Awake()
        {
            base.Awake();

            OnPuzzleExit += HandleOnPuzzleExit;
        }

        protected override void Start()
        {
            base.Start();

            if(finiteStateMachine.CurrentStateId == CompletedState)
            {
                // Open the box cover
                Vector3 eulers = boxCover.transform.localEulerAngles;
                eulers.x = openAngle;
                boxCover.transform.localEulerAngles = eulers;

                // Set as picked
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

            // Push the button
            StartCoroutine(PushButton(interactor.gameObject));

            // Save the last angle id
            lastAngleId = currentAngleId;

            // Rotate the globe
            float speed = 30f;
            float angle = GetNextAngle(interactor.gameObject == leftArrow);
            float time = Mathf.Abs(angle) / speed;
            Debug.Log("NextAngle:" + angle);
            LeanTween.rotateAround(globe, Vector3.up, angle, time);

            yield return new WaitForSeconds(time);


            if (lastDir == 0)
            {
                Debug.Log("First move");
                // We just started rotating the globe
                lastDir = interactor.gameObject == rightArrow ? 1 : -1;
                Debug.Log("Moving direction:" + lastDir);
            }
            else
            {
                // Check if we changed the direction
                if((lastDir == 1 && interactor.gameObject == leftArrow) || (lastDir == -1 && rightArrow == interactor.gameObject))
                {
                    Debug.LogFormat("Switching direction - currentId:{0}, solutionStep:{1}", lastAngleId, solution[step]);
                    // Direction changed, we need to check the angle id
                    if (lastAngleId != solution[step])
                    {
                        // We failed, reset the globe
                        yield return new WaitForSeconds(0.5f); // Wait a little bit
                        time = 0.5f;
                        LeanTween.rotateLocal(globe, Vector3.zero, time);
                        yield return new WaitForSeconds(time);

                        // Send error message
                        GetComponent<Messenger>().SendInGameMessage(6);

                        // Reset fields
                        currentAngleId = 0;
                        step = 0;
                        lastDir = 0;
                    }
                    else
                    {
                        // Update the last direction
                        lastDir = interactor.gameObject == rightArrow ? 1 : -1;

                        // Update the step
                        step++;

                    }
                }

                // Check if puzzle is completed.
                // We simply have to reach the last spot on the globe without changing direction anymore.
                if(step == solution.Length - 1)
                {
                    if(currentAngleId == solution[step])
                    {
                        // Completed
                        SetStateCompleted();

                        GetComponent<Messenger>().SendInGameMessage(12);

                        yield return new WaitForSeconds(1f);

                        openBox = true;

                        Exit();
                    }
                }
            }



            interacting = false;

            OnPuzzleInteractionStop?.Invoke(this);
        }

        IEnumerator PushButton(GameObject button)
        {
            float z = button.transform.localPosition.z;
            float time = 0.125f;
            LeanTween.moveLocalZ(button, z - 0.02f, time);
            yield return new WaitForSeconds(time);
            LeanTween.moveLocalZ(button, z, time);
        }

        float GetNextAngle(bool left)
        {
            float angle;
            if (left)
            {
                if (currentAngleId > 0)
                {
                    angle = angles[currentAngleId - 1] - angles[currentAngleId];
                    currentAngleId--;
                }
                    
                else // Is zero
                {
                    angle = angles[angles.Length - 1] - 360.0f;
                    currentAngleId = angles.Length - 1;
                }
                    
            }
            else
            {
                if(currentAngleId < angles.Length - 1)
                {
                    angle = angles[currentAngleId + 1] - angles[currentAngleId];
                    currentAngleId++;
                }
                else // CurrentId == angles.length - 1
                {
                    angle = 360.0f - angles[currentAngleId];
                    currentAngleId = 0;
                }
            }

            return angle;
        }

        void HandleOnPuzzleExit(PuzzleController controller)
        {
            // It should not happen... but you know.
            if (controller != this)
                return;

            if (!openBox)
                return;

            StartCoroutine(OpenBox());
        }

        IEnumerator OpenBox()
        {
            PlayerManager.Instance.SetDisable(true);

            yield return new WaitForSeconds(0.5f);
            float time = 1f;

            LeanTween.rotateLocal(boxCover, Vector3.right * openAngle, time);

            yield return new WaitForSeconds(time + 0.5f);

            yield return picker.Pick();

            PlayerManager.Instance.SetDisable(false);

        }
    }

}
