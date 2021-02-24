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
                
                if(!playerController) playerController = GetComponent<FirstPersonController>();
                
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
            if (!playerController) playerController = GetComponent<FirstPersonController>();

            Debug.Log("PlayerController:" + playerController);
            playerController.Disabled = value;
        }

        public bool IsDisabled()
        {
            return disabled;
        }

        public void ForceRotation(Quaternion rotation)
        {
            
            bool playerDisabled = disabled;
            
            SetDisable(true);

            transform.rotation = rotation;
            playerController.InitMouseLook();

            // If player was not disable we enable him
            if (!playerDisabled)
                SetDisable(false);
        }

        public void ForcePosition(Vector3 position)
        {
            bool playerDisabled = disabled;

            SetDisable(true);

            transform.position = position;
           
            // If player was not disable we enable him
            if (!playerDisabled)
                SetDisable(false);
        }

    }

}

