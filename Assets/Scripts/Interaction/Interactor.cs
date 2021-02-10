using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Zom.Pie
{
    public abstract class Interactor : MonoBehaviour//, IInteractor
    {
        static List<Interactor> interactors = new List<Interactor>();
        public static IList<Interactor> Interactors
        {
            get { return interactors; }
        }

        public UnityAction<Interactor> OnTriggerEnter;
        public UnityAction<Interactor> OnTriggerExit;

        public abstract void SetEnable(bool value);
               

        protected virtual void Awake()
        {
            interactors.Add(this);
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        private void OnDestroy()
        {
            interactors.Remove(this);
        }
    }

}
