using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;

namespace Zom.Pie
{
    
    public class LibraryCacher : Cacher
    {
        /// <summary>
        /// Creates data to be cached.
        /// </summary>
        /// <returns></returns>
        protected override string GetValue()
        {
            // Read data from the library and build the cache value string.
            string ret = null;
            foreach(Document doc in GetComponent<Library>().Documents)
            {
                if (ret == null)
                {
                    ret = doc.Code;
                }
                else
                {
                    ret += " " + doc.Code;
                }
            }

            return ret;
        }

        /// <summary>
        /// Init object from cache.
        /// </summary>
        /// <param name="value"></param>
        protected override void Init(string value)
        {
            Debug.LogFormat("Cacher - code:{0}, value:{1}", Code, value);
            // Fill the library by reading data from cache.
            string[] codes = value.Split(' ');
            foreach(string code in codes)
            {
                // Get the corresponding resource.
                Document[] docs = Resources.LoadAll<Document>(Constants.DocumentResourceFolder);
                Document doc = new List<Document>(docs).Find(i => i.Code.ToLower().Equals(code.ToLower()));

                // Add the document to the library.
                GetComponent<Library>().Add(doc);
            }
        }

    }

}
