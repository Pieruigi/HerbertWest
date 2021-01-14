using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

        //[MenuItem("Assets/Create/HW/MessageCollection")]
        //public static void CreateMessageCollection()
        //{
        //    MessageCollection asset = ScriptableObject.CreateInstance<MessageCollection>();

        //    string name = "messageCollection.asset";
        //    string folder = "Assets/Resources/MessageCollection/";

        //    if (!System.IO.Directory.Exists(folder))
        //        System.IO.Directory.CreateDirectory(folder);

        //    AssetDatabase.CreateAsset(asset, folder + name);
        //    AssetDatabase.SaveAssets();

        //    EditorUtility.FocusProjectWindow();

        //    Selection.activeObject = asset;
        //}

      
     

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
