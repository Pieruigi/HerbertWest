using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Zom.Pie.Audio
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance { get; private set; }

        [SerializeField]
        ClipData gameClip;

        [SerializeField]
        AudioMixer mixer;

        AudioSource audioSource;

        string musicVolumeParam = "MusicVolume";
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                audioSource = GetComponent<AudioSource>();
                
                // We want the music to keep playing when we go through scenes.
                DontDestroyOnLoad(gameObject);

                //////////////////// TO REMOVE ////////////////////////////
#if UNITY_EDITOR
                PrefsManager.SetMusicVolume(0);
#endif
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // When game starts we start playing music.
            gameClip.Play(audioSource);
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LerpSetVolume(float value, float time)
        {
            float currentVolume;
            mixer.GetFloat(musicVolumeParam, out currentVolume);

            LeanTween.value(gameObject, HandleOnUpdate, currentVolume, value, time);
        }

        public void LerpResetVolume(float time)
        {
            LerpSetVolume(PrefsManager.GetMusicVolume(), time);
        }

        void HandleOnUpdate(float value)
        {
            mixer.SetFloat(musicVolumeParam, value);
        }
    }

}
