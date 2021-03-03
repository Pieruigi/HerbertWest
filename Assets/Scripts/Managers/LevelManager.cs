using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Audio;

namespace Zom.Pie
{
    public class LevelManager : MonoBehaviour
    {
        static LevelManager instance;
        public static LevelManager Instance
        {
            get { return instance; }
        }

        // We can play an audio clip on enter ( for example a door that is closing )
        static ClipData onEnterClip;
        
        
        AudioSource audioSource;

        bool saveOnEnter = true;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
                audioSource = GetComponent<AudioSource>();
            }
            else
            {
                Destroy(gameObject);
            }
            
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

        /// <summary>
        /// Sets the enter clip ( for example a door that is closing )
        /// </summary>
        /// <param name="clip"></param>
        public static void SetOnEnterClip(ClipData clip)
        {
            onEnterClip = clip;
        }

        public static void PlayOnEnterClip()
        {
            if (onEnterClip == null)
                return;

            onEnterClip.Play(instance.audioSource);
        }

        public void PlayClip(ClipData clip)
        {
            if (clip == null)
                return;

            clip.Play(audioSource);
        }

        IEnumerator SpawnPlayer()
        {
            // Play enter clip
            PlayOnEnterClip();

            // Spawn player and disable controller
            PlayerManager.Instance.SetDisable(true);

            if(PlayerSpawner.Instance)
                PlayerSpawner.Instance.Spawn();
            
            // Set black screen and fade in
            CameraFader.Instance.ForceBlackScreen();
            yield return CameraFader.Instance.FadeInCoroutine(2f);

            // Just wait
            yield return new WaitForEndOfFrame();

            // If true the game saves everytime you enter the room
            if(saveOnEnter)
                CacheManager.Instance.Save();

            // Enable player
            PlayerManager.Instance.SetDisable(false);

        }
    }

}
