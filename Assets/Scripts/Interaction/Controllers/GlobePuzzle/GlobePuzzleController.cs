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
        float[] angles;

        int currentId = 0;

        bool interacting = false;


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

            // Rotate the globe
            float speed = 30f;
            float angle = GetNextAngle(interactor.gameObject == leftArrow);
            float time = Mathf.Abs(angle) / speed;
            Debug.Log("NextAngle:" + angle);
            LeanTween.rotateAround(globe, Vector3.up, angle, time);

            yield return new WaitForSeconds(time);

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
                if (currentId > 0)
                {
                    angle = angles[currentId - 1] - angles[currentId];
                    currentId--;
                }
                    
                else // Is zero
                {
                    angle = angles[angles.Length - 1] - 360.0f;
                    currentId = angles.Length - 1;
                }
                    
            }
            else
            {
                if(currentId < angles.Length - 1)
                {
                    angle = angles[currentId + 1] - angles[currentId];
                    currentId++;
                }
                else // CurrentId == angles.length - 1
                {
                    angle = 360.0f - angles[currentId];
                    currentId = 0;
                }
            }

            return angle;
        }
    }

}
