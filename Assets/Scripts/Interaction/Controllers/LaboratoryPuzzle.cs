using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class LaboratoryPuzzle : PuzzleController
    {
        [SerializeField]
        GameObject mixtureLevel;

        [SerializeField]
        AudioSource buttonAudioSource;

        [SerializeField]
        AudioSource liquidAudioSource;

        List<int> solution = new List<int>(4);
        List<int> currentMix = new List<int>(4);
        int currentCount = 0;

        bool interacting = false;

        float buttonMoveDisp = 0.01f;
        float buttonMoveTime = 0.5f;
        float elementButtonMoveDisp = 0.0065f;

        List<Interactor> disableList = new List<Interactor>();

        Picker picker;

        protected override void Awake()
        {
            base.Awake();

            // Init solution array
            solution.Add(0);
            solution.Add(2);
            solution.Add(3);
            solution.Add(4);

            // Set mixture level to zero
            mixtureLevel.transform.localScale = new Vector3(1,0,1);
        }

        protected override void Start()
        {
            base.Start();

            picker = GetComponentInChildren<Picker>();

            if (finiteStateMachine.CurrentStateId == CompletedState)
            {
               
                picker.SetSceneObjectAsPicked();
            }
        }

        public override void Interact(Interactor interactor)
        {
            if (interacting)
                return;

            StartCoroutine(DoInteraction(interactor));
        }


        IEnumerator DoInteraction(Interactor interactor)
        {
            OnPuzzleInteractionStart?.Invoke(this);

            interacting = true;

            //yield return new WaitForSeconds(2);
            // Check the button we pushed
            string suffix = interactor.gameObject.name.Split('_')[1];
            Debug.Log("Suffix:" + suffix);
            if ("mix".Equals(suffix))
            {
                // We are trying to create the mixture
                yield return PressButton(interactor.gameObject, buttonMoveDisp);

                yield return new WaitForSeconds(1);

                // Reset button
                yield return PressButton(interactor.gameObject, -buttonMoveDisp);
                
                // Check
                if (currentCount < 4)
                {
                    // Send ingame message
                    GetComponent<Messenger>().SendInGameMessage(38);

                    
                }
                else
                {
                    yield return ResetMixtureLevel();

                    // Check solution
                    if (CheckSolution())
                    {
                        // Completed
                        GetComponent<Messenger>().SendInGameMessage(40);

                        // Rotate camera
                        Vector3 eulers = Camera.main.gameObject.transform.eulerAngles;
                        eulers.x += 30;
                        float time = 2;
                        LeanTween.rotate(Camera.main.gameObject, eulers, time);
                        yield return new WaitForSeconds(time);

                        yield return picker.Pick();

                        yield return new WaitForSeconds(0.5f);

                        Exit();
                    }
                    else
                    {
                        // Reset 
                        ResetPuzzle();

                        // Send ingame message
                        GetComponent<Messenger>().SendInGameMessage(39);
                    }

                   
                }
                

            }
            else
            {
                if(currentCount == 4)
                {
                    yield return PressButton(interactor.gameObject, elementButtonMoveDisp);
                    GetComponent<Messenger>().SendInGameMessage(41);
                    yield return PressButton(interactor.gameObject, -elementButtonMoveDisp);
                }
                else
                {
                    // Click on element
                    int id = int.Parse(suffix);

                    if (currentMix.Contains(id))
                    {
                        // Already used
                        yield return PressButton(interactor.gameObject, elementButtonMoveDisp);
                        GetComponent<Messenger>().SendInGameMessage(41);
                        yield return PressButton(interactor.gameObject, -elementButtonMoveDisp);
                    }
                    else
                    {
                        // Add the element 
                        currentMix.Add(id);
                        currentCount++;

                        Debug.Log("Selected component:" + id);

                        yield return PressButton(interactor.gameObject, elementButtonMoveDisp);

                        // Disable interactor
                        //disableList.Add(interactor);
                        //interactor.enabled = false;

                        //yield return new WaitForSeconds(3);
                        yield return IncreaseMixtureLevel();

                        yield return PressButton(interactor.gameObject, -elementButtonMoveDisp);
                    }
                   
                }
                
            }

            interacting = false;

            OnPuzzleInteractionStop?.Invoke(this);
        }

        bool CheckSolution()
        {
            if (currentCount < 4)
                return false;

            foreach(int comp in solution)
            {
                if (!currentMix.Contains(comp))
                    return false;
            }

            return true;
        }

        IEnumerator PressButton(GameObject button, float disp)
        {
            float z = button.transform.localPosition.z;
            z -= disp;
            LeanTween.moveLocalZ(button, z, buttonMoveTime).setEaseOutExpo();

            // Audio
            if(disp > 0)
                buttonAudioSource.Play();

            yield return new WaitForSeconds(buttonMoveTime);
        }



        void ResetPuzzle()
        {
            currentCount = 0;
            currentMix.Clear();
            // Enable interactors
            foreach (Interactor interactor in disableList)
                interactor.enabled = true;

            disableList.Clear();

            
        }

        IEnumerator IncreaseMixtureLevel()
        {
            float y = mixtureLevel.transform.localScale.y + 0.25f;
            float time = 3;
            LeanTween.scaleY(mixtureLevel, y, time);

            liquidAudioSource.Play();

            yield return new WaitForSeconds(time);
        }

        IEnumerator ResetMixtureLevel()
        {
            float time = 3;
            LeanTween.scaleY(mixtureLevel, 0, time);

            liquidAudioSource.Play();

            yield return new WaitForSeconds(time);
        }
    }

}
