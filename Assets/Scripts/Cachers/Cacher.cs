using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    /// <summary>
    /// This abstract class implements basic funcionalities to save/load objects to/from cache.
    /// </summary>
    public abstract class Cacher : MonoBehaviour
    {
        [SerializeField]
        string code = "empty";
        public string Code
        {
            get { return code; }
            protected set { code = value; }
        }
       
        /// <summary>
        /// Returns a string which represents the value to be stored in cache.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetValue();

        /// <summary>
        /// Init the object by reading the value of the cache.
        /// </summary>
        /// <param name="cacheValue"></param>
        protected abstract void Init(string value);

        /// <summary>
        /// On Awake() try to read data from cache and to initialize the object.
        /// This way we are sure that the object has already been initialized when you reach Start().
        /// </summary>
        protected virtual void Awake()
        {
           
            // First set the handle to manage updates.
            CacheManager.Instance.OnUpdate += HandleOnCacheUpdate;

            // Init the object
            string value;
            if(CacheManager.Instance.TryGetValue(code, out value))
                Init(value);
        }

        protected virtual void Start()
        {

        }

        /// <summary>
        /// Update cache each time is needed.
        /// </summary>
        void HandleOnCacheUpdate()
        {
            CacheManager.Instance.UpdateValue(code, GetValue());
        }


        /// <summary>
        /// Remove the handle if the object is destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            CacheManager.Instance.OnUpdate -= HandleOnCacheUpdate;
        }
    }

}
