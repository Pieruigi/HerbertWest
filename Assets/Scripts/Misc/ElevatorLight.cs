using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class ElevatorLight : MonoBehaviour
    {

        [SerializeField]
        Material powerOnMaterial;

        [SerializeField]
        FiniteStateMachine fsm;

        int materialId = 1;
        
        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            

            if(fsm.CurrentStateId == 1)
            {
                // Power on
                MeshRenderer r = GetComponent<MeshRenderer>();
                Material[] mats = r.materials;
                mats[materialId] = powerOnMaterial;
                r.materials = mats;

            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
