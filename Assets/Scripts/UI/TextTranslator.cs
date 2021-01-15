using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    /// <summary>
    /// Fill text field with the right text based on the current language.
    /// </summary>
    public class TextTranslator : MonoBehaviour
    {
        [SerializeField]
        TextFactory.Type textType = TextFactory.Type.UILabel;

        [SerializeField]
        int textId;


        // Start is called before the first frame update
        void Start()
        {
            // Fill text field.
            GetComponent<Text>().text = TextFactory.Instance.GetText(textType, textId);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
