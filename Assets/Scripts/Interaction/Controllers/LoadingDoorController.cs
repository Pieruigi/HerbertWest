using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class LoadingDoorController : MonoBehaviour
    {
        [SerializeField]
        int sceneBuildIndex;

        [SerializeField]
        int spawnPointIndex;
        
        FiniteStateMachine fsm;

        int unlocked = 1;
        int locked = 0;



        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
            fsm.OnFail += HandleOnFail;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if(fsm.CurrentStateId == unlocked && fsm.PreviousStateId == unlocked)
            {
                Debug.Log("Loading next scene.");
                //PlayerManager.Instance.SetDisable(true);
                GeneralUtility.LoadScene(this, sceneBuildIndex, spawnPointIndex);
            }
        }

        void HandleOnFail(FiniteStateMachine fsm)
        {
            // If the fsm fails in locked state then we call the ItemUser component in order to 
            // check if there is something in the inventory that we can use.
            if(fsm.CurrentStateId == locked)
            {
                GetComponent<ItemUser>().CheckInventory();
            }
        }
    }

}
