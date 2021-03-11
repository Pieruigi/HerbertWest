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

        [SerializeField]
        bool spawnOnly = false; // False if you want this manager to do other stuff as fade and save

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

            if(PlayerSpawner.Instance)
                PlayerSpawner.Instance.Spawn();

            if (!spawnOnly)
            {
                // Spawn player and disable controller
                PlayerManager.Instance.SetDisable(true);

                // Set black screen and fade in
                CameraFader.Instance.TryDisableAnimator();
                CameraFader.Instance.ForceBlackScreen();

                // Fade in
                yield return CameraFader.Instance.FadeInCoroutine(2f);

                CameraFader.Instance.TryEnableAnimator();

                // Just wait the end of the frame in order to have all the objects initialized
                yield return new WaitForEndOfFrame();

                // If true the game saves everytime you enter the room
                if (GameManager.Instance.InGame && saveOnEnter)
                    CacheManager.Instance.Save();

                // Enable player
                PlayerManager.Instance.SetDisable(false);
            }


        }
    }

}
