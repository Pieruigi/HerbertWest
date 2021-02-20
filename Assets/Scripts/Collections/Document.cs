using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Collections
{
    public class Document : ScriptableObject
    {
        [SerializeField]
        string code;
        public string Code
        {
            get { return code; }
        }

        public string GetDescription()
        {
            DocumentContent info = GetDocumentContent();
            return info.Description;
        }

        public string GetBody()
        {
            DocumentContent info = GetDocumentContent();
            return info.Body;
        }

        DocumentContent GetDocumentContent()
        {
            string fileName = name + "_content_" + GameManager.Instance.Language.ToString();
            return Resources.Load<DocumentContent>(Constants.DocumentResourceFolder + "/" + fileName);
        }
    }

}
