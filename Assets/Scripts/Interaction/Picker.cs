using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;

namespace Zom.Pie
{
    /// <summary>
    /// Can be used as a base class for object picking.
    /// Can be used standalone with its finite state machine or can be controlled by another object
    /// by calling Pick() ( in this case you don't need a fsm attached to this object ).
    /// </summary>
    public abstract class Picker : MonoBehaviour
    {

        [SerializeField]
        GameObject sceneObject;
        protected GameObject SceneObject
        {
            get { return sceneObject; }
        }


        // If you don't need a owned fsm simply call Pick().
        FiniteStateMachine fsm;
        protected FiniteStateMachine FiniteStateMachine
        {
            get { return fsm; }
        }

        int readyState = 1;
        int pickedState = 0;
        protected int PickedState
        {
            get { return pickedState; }
        }



        protected abstract IEnumerator PickEffect();
        protected abstract object GetObject();

        protected virtual void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            if (fsm)
            {
                fsm.OnStateChange += HandleOnStateChange;
            }
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Set state if there is a finite state machine attached.
            if (fsm)
            {
                if(fsm.CurrentStateId == pickedState)
                {
                    if(sceneObject)
                        Destroy(sceneObject);
                }
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        public void SetSceneObjectAsPicked()
        {
            if(sceneObject)
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

            // Effect
            yield return PickEffect();

            // Insert into inventory, or library... or where ever else.
            if (sceneObject)
            {
                Insert(GetObject());
                Destroy(sceneObject);
            }
                
            

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

        void Insert(object obj)
        {
            if(obj.GetType() == typeof(Item))
            {
                Inventory.Instance.Add(obj as Item);
                return;
            }

            if(obj.GetType() == typeof(Document))
            {
                Library.Instance.Add(obj as Document);
            }
                
        }
    }

}
