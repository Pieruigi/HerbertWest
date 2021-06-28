using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class ChairHubController : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> chairs;

        [SerializeField]
        GameObject door;

        [SerializeField]
        GameObject gate;

        [SerializeField]
        Collider gateCollider;

        int[] solution;

        private void Awake()
        {
           

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
            foreach (GameObject chair in chairs)
            {
                chair.GetComponentInChildren<FiniteStateMachine>().OnStateChange += HandleOnStateChange;
            }

            // If is completed we open the gate
            if (IsCompleted())
            {
                OpenGate();
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
                    chair.GetComponentInChildren<FiniteStateMachine>().ForceStateDisabled();


                // Open door
                OpenDoor();

                // Open gate
                OpenGate();

                CacheManager.Instance.Save();
            }
            
        }

        bool IsCompleted()
        {
            for (int i = 0; i < chairs.Count; i++)
            {
                if (chairs[i].GetComponentInChildren<ChairController>().GetSymbol() != solution[i])
                    return false;
            }

            return true;
        }

        void OpenGate()
        {
            // Disable collider
            gateCollider.enabled = false;

            // Open gate
            LeanTween.moveLocalY(gate, -4.77f, 0.5f).setEaseInExpo();
        }

        void OpenDoor()
        {
            // Some audio


            // Set door open
            door.GetComponent<FiniteStateMachine>().ForceState(1, false, false);

            StartCoroutine(SendMessageDoorUnlocked());

            
        }

        IEnumerator SendMessageDoorUnlocked()
        {
            yield return new WaitForSeconds(1);

            // Send message
            door.GetComponent<FiniteStateMachineMessenger>().SendInGameMessage(10);
        }
    }

}
