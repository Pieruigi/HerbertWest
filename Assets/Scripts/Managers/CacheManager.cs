using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class CacheManager : MonoBehaviour
    {
        public static CacheManager Instance { get; private set; }

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

        /// <summary>
        /// Returns true if there is a saved game.
        /// </summary>
        /// <returns></returns>
        public bool IsSaveGameAvailable()
        {
            return false;
        }

        /// <summary>
        /// Clear cache and delete all save games.
        /// </summary>
        public void Delete()
        {

        }

        public void Load()
        {

        }

        public bool TryGetCacheValue(string cacheName, out string cacheValue)
        {
            cacheValue = "0";
            return true;
        }
    }

}
