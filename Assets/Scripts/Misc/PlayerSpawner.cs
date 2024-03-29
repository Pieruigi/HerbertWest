﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class PlayerSpawner: MonoBehaviour
    {
       
        [SerializeField]
        List<Transform> spawnPoints;

        static int spawnPointId = 0; // 0 is the player starting point id for a new game
        public static int SpawnPointId
        {
            get { return spawnPointId; }
            set { spawnPointId = value; }
        }

        public static PlayerSpawner Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
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


        public void Spawn()
        {
            //PlayerManager.Instance.SetDisable(true);

            // Ask the level manager for the spawn point in the world
            if (spawnPointId < 0)
            {
                // Use cache
                PlayerCacher cacher = PlayerManager.Instance.GetComponent<PlayerCacher>();
                PlayerManager.Instance.transform.position = cacher.GetPosition();
                PlayerManager.Instance.transform.eulerAngles = cacher.GetEulerAngles();
            }
            else
            {
                // Use spawn point
                Transform sp = spawnPoints[spawnPointId];
                Debug.Log("SpawnPoint:" + sp);

                // Set position and rotation
                //PlayerManager.Instance.transform.position = sp.position;
                //PlayerManager.Instance.transform.rotation = sp.rotation;
                PlayerManager.Instance.ForcePosition(sp.position);
                PlayerManager.Instance.ForceRotation(sp.rotation);

                spawnPointId = -1;

                //PlayerManager.Instance.SetDisable(false);
            }

        }
        
    }

}
