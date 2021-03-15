using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Audio;
using Zom.Pie.Collections;

namespace Zom.Pie
{
    public class LoadingDoorController : MonoBehaviour
    {
        [SerializeField]
        int sceneBuildIndex;

        [SerializeField]
        int spawnPointIndex;

        [SerializeField]
        Item key;

        [SerializeField]
        ClipData lockedClip;

        [SerializeField]
        ClipData unlockClip;

        [SerializeField]
        ClipData openClip;

        [SerializeField]
        ClipData closeClip;

        FiniteStateMachine fsm;

        int unlocked = 1;
        int locked = 0;

        InventoryUser inventoryUser;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
            fsm.OnFail += HandleOnFail;

        }

        // Start is called before the first frame update
        void Start()
        {
            // If the door is locked then create the instance to open inventory and choose the key.
            if(fsm.CurrentStateId == locked)
            {
                inventoryUser = new InventoryUser(this, HandleOnItemChosen);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if(fsm.CurrentStateId == unlocked && fsm.PreviousStateId == unlocked)
            {
                Debug.LogFormat("Loading next scene - sceneId:{0}, spawnIndex:{1}", sceneBuildIndex, spawnPointIndex);
                // Set the clip you want to play entering the next room
                LevelManager.SetOnEnterClip(closeClip);
                
                // Play open clip
                LevelManager.Instance.PlayClip(openClip);
                GeneralUtility.LoadScene(this, sceneBuildIndex, spawnPointIndex);
            }
        }

        void HandleOnFail(FiniteStateMachine fsm)
        {
            // If the fsm fails in locked state then we call the ItemUser component in order to 
            // check if there is something in the inventory that we can use.
            if(fsm.CurrentStateId == locked)
            {
                // Play audio
                LevelManager.Instance.PlayClip(lockedClip);

                // Open inventory to check for items.
                inventoryUser.Open();
            }
        }

        void HandleOnItemChosen(Item item)
        {
            //ItemUser iu = GetComponent<ItemUser>();
            if(key == null || key != item)
            {
                // Tell the inventory we chosen a wrong item
                inventoryUser.ReportWrongItem();
            }
            else
            {
                // Report the inventory we chosen the right item.
                inventoryUser.ReportRightItem();
                // Remove the item
                Inventory.Instance.Remove(item);
                // Force door unlocked
                fsm.ForceState(unlocked, true, true);

                // Play audio
                LevelManager.Instance.PlayClip(unlockClip);

                // Show message
                GetComponent<Messenger>().SendInGameMessage(26);
            }
            
        }
    }

}
