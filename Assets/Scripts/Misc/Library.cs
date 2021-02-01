using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;

namespace Zom.Pie
{
    public class Library : MonoBehaviour
    {
        List<Document> documents = new List<Document>();
        public IList<Document> Documents
        {
            get { return documents.AsReadOnly(); }
        }

        public static Library Instance { get; private set; }

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

        public void Add(Document document)
        {
            documents.Add(document);
        }
    }

}
