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


        [MenuItem("Assets/Create/Collections/Item")]
        public static void CreateItem()
        {
            Item asset = ScriptableObject.CreateInstance<Item>();
            ItemInfo assetInfo = ScriptableObject.CreateInstance<ItemInfo>();

            string name = "/empty.asset";
            string nameInfo = "/empty_info.asset";

            string folder = System.IO.Path.Combine("Assets/Resources", Constants.ItemResourceFolder);

            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            AssetDatabase.CreateAsset(asset, folder + name);
            AssetDatabase.CreateAsset(assetInfo, folder + nameInfo);

            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }

        [MenuItem("Assets/Create/Collections/Document")]
        public static void CreateUIMessageCollection()
        {
            Document asset = ScriptableObject.CreateInstance<Document>();
            DocumentContent assetContent = ScriptableObject.CreateInstance<DocumentContent>();

            string name = "/document.asset";
            string nameContent = "/document_content.asset";
            string folder = System.IO.Path.Combine("Assets/Resources", Constants.DocumentResourceFolder);

            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            AssetDatabase.CreateAsset(asset, folder + name);
            AssetDatabase.CreateAsset(assetContent, folder + nameContent);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }


    }

}
