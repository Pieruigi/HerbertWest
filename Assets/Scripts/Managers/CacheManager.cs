using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

namespace Zom.Pie
{
    public class CacheManager 
    {
        /// <summary>
        /// Called each time cache need to updated ( commonly when game saves progress ).
        /// </summary>
        public UnityAction OnUpdate;

        /// <summary>
        /// Called after cache has been stored on disk ( save game ).
        /// </summary>
        //public UnityAction OnSave;

        Dictionary<string, string> cache;

        private string folder = Application.persistentDataPath + "/Saves/";
        private string file = "sav.txt";

        private static CacheManager instance;
        public static CacheManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new CacheManager();

                return instance;
            }
        }
        private CacheManager()
        {
            Debug.Log("Creating cache system...");

            // Init cache list
            cache = new Dictionary<string, string>();

            // Create the base folder if needed ( the file we'll be created on the first saving )
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);


            ////////// ONLY FOR TEST /////////////////////////////////
            //Load();

            //// Create save file if needed
            //string path = Path.Combine(folder, file);
            //if (!File.Exists(path))
            //    using (FileStream fs = File.Create(path)) { }

        }


        /// <summary>
        /// Returns true if there is a saved game.
        /// </summary>
        /// <returns></returns>
        public bool IsSaveGameAvailable()
        {
            string path = Path.Combine(folder, file);
            return File.Exists(path);
        }


        /// <summary>
        /// Adds or update a given object in cache.
        /// </summary>
        /// <param name="code">The object is being updated.</param>
        /// <param name="data">The cache value.</param>
        public void UpdateValue(string code, string data)
        {
            if (!cache.ContainsKey(code))
                cache.Add(code, data);
            else
                cache[code] = data;

        }

        /// <summary>
        /// Returns true if the object given as param exits in cache, otherwise returns false.
        /// </summary>
        /// <param name="code">The object you are looking for.</param>
        /// <param name="value">The returning value.</param>
        /// <returns>True if the object exists, otherwise false.</returns>
        public bool TryGetValue(string code, out string value)
        {
            value = null;

            // Key doesn't exist
            if (!cache.ContainsKey(code))
                return false;

            // Fill the out param and return true
            value = cache[code];
            return true;

        }

        /// <summary>
        /// Updates cache without writing on disk.
        /// </summary>
        public void Update()
        {
            OnUpdate?.Invoke();
        }

        /// <summary>
        /// Write cache on disk.
        /// This method first call update in order to let all the objects that need to be stored to update cache.
        /// </summary>
        public void Save()
        {
            //OnCacheUpdate?.Invoke();
            Update();

            WriteCache();

            //OnSave?.Invoke();
        }

        /// <summary>
        /// Clear cache and delete all save games.
        /// </summary>
        public void Delete()
        {
            // Delete cache.
            cache.Clear();

            // Delete file if exists.
            string path = Path.Combine(folder, file);
            if (File.Exists(path))
                File.Delete(path);

        }

        /// <summary>
        /// Load data from disk.
        /// </summary>
        public void Load()
        {
            // Clear cache.
            cache.Clear();
            // Read from file.
            ReadCache();
        }


        #region private
        /// <summary>
        /// Read cache from disk and fill dictionary.
        /// </summary>
        private void ReadCache()
        {
            string fileTxt = File.ReadAllText(Path.Combine(folder, file));

            using (StringReader sr = new StringReader(fileTxt))
            {
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    string code = line.Substring(0, line.IndexOf(' '));
                    string value = line.Substring(line.IndexOf(' ')+1, line.Length - line.IndexOf(' ') - 1);
                    //string[] s = line.Split(' ');
                    cache.Add(code, value);
                }
            }

        }

        /// <summary>
        /// Store cache on disk.
        /// </summary>
        private void WriteCache()
        {
            // Create save file if it doesn't exist
            string path = Path.Combine(folder, file);
            if (!File.Exists(path))
                using (FileStream fs = File.Create(path)) { }

            using (StringWriter sw = new StringWriter())
            {
                List<string> keys = new List<string>(cache.Keys);
                foreach (string key in keys)
                {
                    sw.WriteLine(key + " " + cache[key]);
                }

                File.WriteAllText(Path.Combine(folder, file), sw.ToString());
            }
        }

        #endregion
    }

}
