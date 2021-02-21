using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.UI;
using Zom.Pie.Collections;

namespace Zom.Pie
{
    /// <summary>
    /// In game document reading system.
    /// </summary>
    public class InGameReaderController : MonoBehaviour
    {
        [SerializeField]
        Document document;

        FiniteStateMachine fsm;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            // We avoid to configure the finite state machine and simply we handle the error.
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

        void HandleOnFail(FiniteStateMachine fsm)
        {
            // This really should not be necessary... but you know.
            if (fsm.CurrentStateId < 0)
                return;

            OpenUI();
        }

        void OpenUI()
        {
            // Open the in game UI
            InGameReaderUI.Instance.Open(document);
        }


    }

}