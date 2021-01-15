using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zom.Pie
{
    public class LevelCacher : Cacher
    {

        protected override void Awake()
        {
            // Change cache code.
            Code = Constants.CacheCodeSceneIndex;

            // Call parent.
            base.Awake();
        }

        protected override string GetValue()
        {
            return SceneManager.GetActiveScene().buildIndex.ToString();
        }

        protected override void Init(string value)
        {
            // Nothing to do.
        }
    }

}
