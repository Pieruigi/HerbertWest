using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class ChairHubController : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> chairs;

        int[] solution;

        private void Awake()
        {
            foreach(GameObject chair in chairs)
            {
                chair.GetComponent<FiniteStateMachine>().OnStateChange += HandleOnStateChange;
            }

            // Set solution
            solution = new int[6];
            solution[0] = (int)ChairController.Symbol.Sword;
            solution[1] = (int)ChairController.Symbol.Shield;
            solution[2] = (int)ChairController.Symbol.Coin;
            solution[3] = (int)ChairController.Symbol.Snake;
            solution[4] = (int)ChairController.Symbol.Fish;
            solution[5] = (int)ChairController.Symbol.Bow;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (IsCompleted())
            {
                // Set box opened

                // Set object picked
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            // Check            
            if (IsCompleted())
            {
                // Force each chair disabled
                foreach (GameObject chair in chairs)
                    chair.GetComponent<FiniteStateMachine>().ForceStateDisabled();


                // Open box
                StartCoroutine(OpenBox());
            }
            
        }

        bool IsCompleted()
        {
            for (int i = 0; i < chairs.Count; i++)
            {
                if (chairs[i].GetComponent<ChairController>().GetSymbol() != solution[i])
                    return false;
            }

            return true;
        }

        IEnumerator OpenBox()
        {
            yield break;
        }
    }

}
