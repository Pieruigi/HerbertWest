using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class LevelManager : MonoBehaviour
    {
        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(SpawnPlayer());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator SpawnPlayer()
        {
            // Spawn player and disable controller
            PlayerSpawner.Instance.Spawn();
            PlayerManager.Instance.SetDisable(true);

            // Set black screen and fade in
            CameraFader.Instance.ForceBlackScreen();
            yield return CameraFader.Instance.FadeInCoroutine(2f);

            // Enable player
            PlayerManager.Instance.SetDisable(false);

        }
    }

}
