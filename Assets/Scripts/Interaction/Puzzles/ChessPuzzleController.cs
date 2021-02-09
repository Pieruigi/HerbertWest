using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class ChessPuzzleController : PuzzleController
    {
        
        [SerializeField]
        List<GameObject> pieces;

        
        protected override void Start()
        {
            base.Start();

            if(finiteStateMachine.CurrentStateId == CompletedState)
            {
                // Set the chess open
            }
            else
            {
                // Set buttons 
                
                for(int i=0; i<pieces.Count; i++)
                {
                    Vector3 pos = pieces[i].transform.position;
                    if (i == 0) // Is the horse
                    {
                        pos.y += 0.02f;
                    }
                    else
                    {
                        pos.y -= 0.018f;
                    }

                    pieces[i].transform.position = pos;
                }
            }
        }

        bool busy = false;
        public override void Interact(Interactor interactor)
        {
            if (busy)
                return;

            busy = true;

            Debug.Log("Interacting with " + interactor.gameObject);
        }

        IEnumerator DoInteraction()
        {
            OnPuzzleInteractionStart?.Invoke(this);

            yield return new WaitForSeconds(1f);

            OnPuzzleInteractionStop?.Invoke(this);
        }
    }

}
