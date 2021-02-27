using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class ElectricBoxPuzzleController : PuzzleController
    {
        [SerializeField]
        Transform fuseSpareContainer;

        [SerializeField]
        Transform spotContainer; 

        List<GameObject> fuses;

        List<Transform> spots;

        Dictionary<GameObject, Vector3> defaultPositions; // Fuses default positions

        // Each id represent the id of the corresponding fuse in the fuse list; the value is -1 if the fuse
        // is in its default position othervise is the id of the spot the fuse has been inserted in the box.
        List<int> placings; 

        bool interacting = false;

        GameObject selectedFuse = null;

        protected override void Start()
        {
            base.Start();

            // Fill the fuse list and store default position
            fuses = new List<GameObject>();
            defaultPositions = new Dictionary<GameObject, Vector3>();
            placings = new List<int>();
            for(int i=0; i<fuseSpareContainer.childCount; i++)
            {
                GameObject fuse = fuseSpareContainer.GetChild(i).gameObject;

                // Add fuse
                fuses.Add(fuse);

                // Store initial position
                defaultPositions.Add(fuse, fuse.transform.localPosition);

                // Each fuse starts from its default position ( in the spare container )
                placings.Add(-1);
            }

            // Get all the spots
            spots = new List<Transform>();
            for (int i = 0; i < spotContainer.childCount; i++)
            {
                spots.Add(spotContainer.GetChild(i));
            }
                


            // Check wheather the state is ready, compleated or missing.
            if(finiteStateMachine.CurrentStateId == 2)
            {
                // In missing state, check missing fuses which are the ones with the fsm attached to them.
                List<GameObject> missingFuses = fuses.FindAll(f => f.GetComponent<FiniteStateMachine>());

                // If we picked both then we set the state to ready
                if (AllFusesPicked(missingFuses))
                    finiteStateMachine.ForceState(ReadyState, false, false);
            }
            
        }

        public override void Interact(Interactor interactor)
        {
            if (interacting)
                return;

            StartCoroutine(DoInteraction(interactor));


        }

        bool AllFusesPicked(List<GameObject> missings)
        {
            foreach (GameObject fuse in missings)
                if (fuse.GetComponent<FiniteStateMachine>().CurrentStateId != 0)
                    return false;

            return true;
        }

        IEnumerator DoInteraction(Interactor interactor)
        {
            OnPuzzleInteractionStart?.Invoke(this);

            interacting = true;

            if (IsFuse(interactor.gameObject))
            {
                // We must check whether is already set or not ( is in spot or in the spare )
                if (IsInSpare(interactor.gameObject))
                {
                    Debug.LogFormat("Fuse {0} is in the spare", interactor.name);


                    if(selectedFuse == null || selectedFuse != interactor.gameObject)
                    {
                        Vector3 pos = Vector3.zero;
                        if (selectedFuse != null)
                        {
                            // We have already a fuse selected, so we must unselect it first
                            // Move it back
                            defaultPositions.TryGetValue(selectedFuse, out pos);
                            LeanTween.moveLocalZ(selectedFuse, pos.z, 0.5f);

                            // Enable collider
                            selectedFuse.GetComponent<Collider>().enabled = true;
                        }

                        // Set as selected
                        selectedFuse = interactor.gameObject;

                        // Move a little
                        defaultPositions.TryGetValue(selectedFuse, out pos);
                        LeanTween.moveLocalZ(selectedFuse, pos.z-0.03f, 0.5f);

                        // Disable interaction
                        selectedFuse.GetComponent<Collider>().enabled = false;
                    }



                }
                else
                {
                    Debug.LogFormat("Fuse {0} is in spot {1}", interactor.name, GetSpotId(interactor.gameObject));
                }
            }

            yield return new WaitForSeconds(1f);

            interacting = false;

            OnPuzzleInteractionStop?.Invoke(this);

        }

        bool IsFuse(GameObject obj)
        {
            return obj.name.StartsWith("Fuse_");
        }

        int GetFuseType(GameObject fuse)
        {
            return int.Parse(fuse.name.Split('_')[1]);
        }

        bool IsInSpare(GameObject fuse)
        {
            int id = fuses.IndexOf(fuse);
            return placings[id] < 0;
        }

        int GetSpotId(GameObject fuse)
        {
            int id = fuses.IndexOf(fuse);
            return placings[id];
        }
    }

}
