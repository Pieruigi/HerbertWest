using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Zom.Pie.Collections;

namespace Zom.Pie.Editor
{

    public class AssetBuilder : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        [MenuItem("Assets/Create/Collections/TextCollection")]
        public static void CreateTextCollection()
        {
            TextCollection asset = ScriptableObject.CreateInstance<TextCollection>();

            string name = "/empty.asset";
            string folder = System.IO.Path.Combine("Assets/Resources", TextFactory.ResourceFolder);

            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            AssetDatabase.CreateAsset(asset, folder + name);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }




        //[MenuItem("Assets/Create/HW/UI/MessageCollection")]
        //public static void CreateUIMessageCollection()
        //{
        //    MessageCollection asset = ScriptableObject.CreateInstance<MessageCollection>();

        //    string name = "messageCollection.asset";
        //    string folder = "Assets/Resources/" + Constants.ResourceFolderMessageCollectionUI +"/";

        //    if (!System.IO.Directory.Exists(folder))
        //        System.IO.Directory.CreateDirectory(folder);

        //    AssetDatabase.CreateAsset(asset, folder + name);
        //    AssetDatabase.SaveAssets();

        //    EditorUtility.FocusProjectWindow();

        //    Selection.activeObject = asset;
        //}


    }

}
