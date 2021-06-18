using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Audio
{
    [System.Serializable]
    public class ClipData
    {
        [SerializeField]
        AudioClip clip;
        public AudioClip Clip
        { get { return clip; } }


        [SerializeField]
        float volume = 1;
        public float Volume
        {
            get { return volume; }
        }

        [SerializeField]
        bool loop = false;
        public bool Loop
        {
            get { return loop; }
        }

        public void Play(AudioSource audioSource)
        {
            if (!audioSource)
                return;

            Init(audioSource);

            audioSource.Play();
        }

        public void PlayDelayed(AudioSource audioSource, float delay)
        {
            if (!audioSource)
                return;

            Init(audioSource);

            audioSource.PlayDelayed(delay);
        }

        

        void Init(AudioSource audioSource)
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.loop = loop;
        }
    }

}
