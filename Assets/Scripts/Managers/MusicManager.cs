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
        ClipData[] clips;

        [SerializeField]
        int currentClipId;
        public int CurrentClipId
        {
            get { return currentClipId; }
        }

        [SerializeField]
        AudioMixer mixer;

        AudioSource audioSource;

        string musicVolumeParam = "MusicVolume";

        float fadeTime = 2;
        float musicVolume;
        public float MusicVolume
        {
            get { return musicVolume; }
        }
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                audioSource = GetComponent<AudioSource>();

                if (PlayerPrefs.HasKey(musicVolumeParam))
                {
                    musicVolume = PlayerPrefs.GetFloat(musicVolumeParam);
                }
                else
                {
                    musicVolume = 1;
                }
#if UNITY_EDITOR
                musicVolume = 1;
#endif

                // We want the music to keep playing when we go through scenes.
                DontDestroyOnLoad(gameObject);

                
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            //if (GameManager.Instance.InGame)
            //{
            //    // Try play level music
            //}

            //// When game starts we start playing music.
            //clips[currentClipId].Play(audioSource);
            mixer.SetFloat(musicVolumeParam, GeneralUtility.VolumeToDecibel(musicVolume));
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PlayMusic(int clipId)
        {
            Debug.Log("Playing music...");
            // Already playing the same music
            if (IsPlaying() && currentClipId == clipId)
                return;

            Debug.Log("IsPlaying():" + IsPlaying());
            // Not playing yet, simply start playing 
            if (!IsPlaying())
            {
                SetClip(clipId);
                // Set volume to zero 
                mixer.SetFloat(musicVolumeParam, GeneralUtility.VolumeToDecibel(0));
                audioSource.Play();
                // Fade in
                LerpResetVolume(fadeTime);
                return;
            }

            Debug.Log("Is playing another clip:" + audioSource.clip);
            // Is playing another clip
            if(clipId >= 0)
            {
                StartCoroutine(SwitchMusic(clipId));
            }
            else
            {
                mixer.SetFloat(musicVolumeParam, GeneralUtility.VolumeToDecibel(musicVolume));
                audioSource.Stop();
                audioSource.clip = null;
            }

        }

        public void ForceStopMusic()
        {
            audioSource.Stop();
            mixer.SetFloat(musicVolumeParam, GeneralUtility.VolumeToDecibel(musicVolume));
            
        }

        void SetClip(int clipId)
        {
            currentClipId = clipId;
            audioSource.clip = clips[currentClipId].Clip;
            audioSource.volume = clips[currentClipId].Volume;
            audioSource.loop = true;
        }

        bool IsPlaying()
        {
            return audioSource.isPlaying;
        }

        /// <summary>
        /// Use this if you want for example the music to fade out when playing a puzzle.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="time"></param>
        public void LerpSetVolume(float value, float time)
        {
            float currentVolume;
            mixer.GetFloat(musicVolumeParam, out currentVolume);
            currentVolume = GeneralUtility.DecibelToVolume(currentVolume);

            LeanTween.value(gameObject, HandleOnUpdate, currentVolume, value, time);
        }

        /// <summary>
        /// Use this if you want for example the music to fade in after you closed a puzzle
        /// </summary>
        /// <param name="time"></param>
        public void LerpResetVolume(float time)
        {
            LerpSetVolume(musicVolume, time);
        }

        void HandleOnUpdate(float value)
        {
            mixer.SetFloat(musicVolumeParam, GeneralUtility.VolumeToDecibel(value));
            //audioSource.volume = value;
        }

        IEnumerator SwitchMusic(int newClipId)
        {
            LerpSetVolume(0, fadeTime);

            // Wait fade out
            yield return new WaitForSeconds(fadeTime + 1);

            audioSource.Stop();

            // Set the new clip
            SetClip(newClipId);

            audioSource.Play();

            // Fade in
            LerpResetVolume(fadeTime);
        }

        
    }

}
