using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class PrefsManager
    {
        static readonly string MusicVolumeKey = "MusicVolume";

        public static void SetMusicVolume(float value)
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, value);
        }

        public static float GetMusicVolume()
        {
            return PlayerPrefs.GetFloat(MusicVolumeKey);
        }
    }

}
