using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public enum Language { English, Italian }

    public class Constants 
    {
        public static readonly string CacheCodeSceneIndex = "s";


        // Resources
        public static readonly string ItemResourceFolder = "Items";
        public static readonly string DocumentResourceFolder = "Documents";
        public static readonly string ClueResourceFolder = "Clues";

        // Audio
        public static readonly float OnPuzzleMusicVolume = 0.75f;
        public static readonly float OnPuzzleMusicFade = 1f;
    }

}
