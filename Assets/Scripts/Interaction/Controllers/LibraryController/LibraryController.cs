using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class LibraryController : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> books;

        [SerializeField]
        GameObject paint;

        FiniteStateMachine fsm;

        int completedState = 0;

        List<int> solution;
        int count = 0;
        float paintDisp = 1.06f;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();

            // Set solution
            solution = new List<int>();
            solution.Add(0);
            solution.Add(1);
            solution.Add(2);
        }

        // Start is called before the first frame update
        void Start()
        {

            // Set handle for each book 
            foreach(GameObject book in books)
            {
                // Set handles
                FiniteStateMachine bookFsm = book.GetComponent<FiniteStateMachine>();
                bookFsm.OnStateChange += HandleOnStateChange;
            }

            if(fsm.CurrentStateId == completedState)
            {
                // Set new paint position
                Vector3 pos = paint.transform.position;
                pos.x += paintDisp;
                paint.transform.position = pos;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if (fsm.CurrentStateId != LibraryBookController.PushedState)
                return;

            // Get the id of the book we pushed
            int bookId = books.FindIndex(b => b == fsm.gameObject);

            // Check next book
            int nextId = solution[count];
            
            if(nextId == bookId)
            {
                // Ok we pushed the right one
                count++;

                if(count == books.Count)
                {
                    // We have completed the puzzle
                    StartCoroutine(Succeed());
                }
            }
            else
            {
                // This is not the right book, we failed

                StartCoroutine(Fail());
                // Reset all the books

            }
            
        }

        IEnumerator Fail()
        {
            count = 0;

            yield return new WaitForSeconds(1f);


            // Reset all the books
            // The reason why we set state to -1 is that we don't want the player to be able to click
            // when button is doing animation; the button will reset its state by his hand at the end of
            // the animation
            foreach (GameObject book in books)
                book.GetComponent<FiniteStateMachine>().ForceState(-1, true, false);
        }

        IEnumerator Succeed()
        {
            yield return new WaitForSeconds(1f);

            // Set the state
            fsm.ForceState(completedState, false, false);

            // Move paint
            LeanTween.moveX(paint, paint.transform.position.x + paintDisp, 0.5f).setEaseOutExpo();
        }
    }

}
