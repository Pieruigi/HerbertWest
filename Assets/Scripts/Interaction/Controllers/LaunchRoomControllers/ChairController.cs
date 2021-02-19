using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Interfaces;

namespace Zom.Pie
{
    public class ChairController : MonoBehaviour, ICacheable
    {
        public enum Symbol { Coin, Snake, Fish, Sword, Shield, Bow }

        [SerializeField]
        Symbol symbol;
        

        FiniteStateMachine fsm;

        int stateAvailable = 0;



        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;

            // Rotate
            float angle = -60.0f * (float)symbol;
            Vector3 eulers = transform.localEulerAngles;
            eulers.z = angle;
            transform.localEulerAngles = eulers;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public int GetSymbol()
        {
            return (int)symbol;
        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if (fsm != this.fsm)
                return;

            if (fsm.CurrentStateId != stateAvailable)
                return;

            // Switch symbol
            int count = (int)symbol;
            count++;
            if (count > 5)
                count = 0;
            symbol = (Symbol)count;

            // Rotate
            float angle = transform.localEulerAngles.z - 60.0f;

            LeanTween.rotateZ(gameObject, angle, 0.75f).setEaseInOutExpo().setEaseOutElastic();//.setEaseInOutElastic();
        }

        public string GetData()
        {
            return ((int)symbol).ToString();
        }

        public void Init(string extraData)
        {
            symbol = (Symbol)int.Parse(extraData);
        }
    }

}