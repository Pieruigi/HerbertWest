using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Zom.Pie
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }

        FirstPersonController playerController;

        bool disabled = false;
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                playerController = GetComponent<FirstPersonController>();
                
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        /// <summary>
        /// Enable/disable player.
        /// </summary>
        /// <param name="value"></param>
        public void SetDisable(bool value)
        {
            disabled = value;
            playerController.Disabled = value;
        }

        public bool IsDisabled()
        {
            return disabled;
        }

    }

}

