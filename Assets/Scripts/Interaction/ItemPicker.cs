using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;

namespace Zom.Pie
{
    public class ItemPicker : MonoBehaviour
    {
        [SerializeField]
        Item item;

        [SerializeField]
        GameObject sceneObject;

        [SerializeField]
        float distance;

        float time = 1f;

        // The current state machine attached to the current object if any.
        // If you don't need a owned fsm simply call Pick().
        FiniteStateMachine fsm;

        int readyState = 1;
        int pickedState = 0;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            if (fsm)
            {
                fsm.OnStateChange += HandleOnStateChange;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Set state if there is a finite state machine attached.
            if (fsm)
            {
                if(fsm.CurrentStateId == pickedState)
                {
                    SetAsPicked();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetAsPicked()
        {
            if (fsm)
                fsm.ForceState(pickedState, false, false);
           
            Destroy(sceneObject);
        }

        public IEnumerator Pick()
        {
            // If the player has already been disabled by another script ( puzzle for example ), then we must
            // skip the SetDisable() method.
            bool playerdisabled = PlayerManager.Instance.IsDisabled();

            if (!playerdisabled)
            {
                PlayerManager.Instance.SetDisable(true);
            }

            // If picker is used with a finite state machine then we set the new state.
            if (fsm)
            {
                fsm.ForceState(pickedState, true, true);
            }

            LeanTween.move(sceneObject, Camera.main.transform.position + Camera.main.transform.forward * distance, time);

            yield return new WaitForSeconds(time+2f);

            SetAsPicked();

            Inventory.Instance.Add(item);

            // If the player was already disabled when we start picking then we don't enable him back 
            // from here.
            if (!playerdisabled)
            {
                PlayerManager.Instance.SetDisable(false);
            }
        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if (fsm.CurrentStateId == pickedState && fsm.PreviousStateId == readyState)
                StartCoroutine(Pick());
        }
    }

}
