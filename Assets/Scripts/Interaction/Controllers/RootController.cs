using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;
using Zom.Pie.UI;

namespace Zom.Pie
{
    public class RootController : MonoBehaviour
    {

        [SerializeField]
        Item itemToUse;

        [SerializeField]
        GameObject rootObject;

        [SerializeField]
        AudioSource audioSource;

        FiniteStateMachine fsm;

        bool interacting = false;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
        }

        // Start is called before the first frame update
        void Start()
        {


            if(fsm.CurrentStateId == 0)
            {
                rootObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

            
        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if(fsm.CurrentStateId == 1 && fsm.PreviousStateId == 1)
            {
                if (interacting)
                    return;

                StartCoroutine(DoInteraction());
            }
        }

        IEnumerator DoInteraction()
        {
            interacting = true;

            // Send message
            GetComponent<FiniteStateMachineMessenger>().SendInGameMessage(37);

            // Wait
            yield return new WaitForSeconds(2f);

            // Open inventory
            InventoryUI.Instance.OnItemChosen += HandleOnItemChosen;
            InventoryUI.Instance.OnClosed += HandleOnInventoryClosed;
            GameManager.Instance.OpenInventory(true);

            interacting = false;
        }

        void HandleOnItemChosen(Item item)
        {
            
            if(item == itemToUse)
            {
                // Close inventory ui
                GameManager.Instance.CloseInventory();

                // Remove the item 
                Inventory.Instance.Remove(item);

                // Remove root and set new picker state
                fsm.ForceState(0, false, true);


                // Effect
                StartCoroutine(RemoveRoot());
            }
            else
            {
                InventoryUI.Instance.ShowWrongItemMessage();
            }
        }

        void HandleOnInventoryClosed()
        {
            InventoryUI.Instance.OnItemChosen -= HandleOnItemChosen;
            InventoryUI.Instance.OnClosed -= HandleOnInventoryClosed;
        }

        IEnumerator RemoveRoot()
        {
            PlayerManager.Instance.SetDisable(true);

            CameraFader.Instance.TryDisableAnimator();
            yield return CameraFader.Instance.FadeOutCoroutine(1f);

            rootObject.SetActive(false);

            audioSource.Play();

            yield return new WaitForSeconds(5);

            yield return CameraFader.Instance.FadeInCoroutine(1f);
            CameraFader.Instance.TryEnableAnimator();

            PlayerManager.Instance.SetDisable(false);
        }
    }

}
