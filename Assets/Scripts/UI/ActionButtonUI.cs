using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Zom.Pie.UI
{
    public class ActionButtonUI : MonoBehaviour
    {
        [SerializeField]
        Color32 enabledColor;

        [SerializeField]
        Color32 disabledColor;

        Image image;

        Vector3 sizeDefault;
        Vector3 sizeAction;
        float scaleTime = 0.2f;


        void Awake()
        {
            image = GetComponent<Image>();
            image.color = disabledColor;

            sizeDefault = transform.localScale;
            sizeAction = 1.5f * sizeDefault;
        }

        // Start is called before the first frame update
        void Start()
        {
            // We get all the interactors in order to enable or disable.
            foreach(Interactor interactor in Interactor.Interactors)
            {
                // If the interactor is a puzzle child then skip.
                if (interactor.GetComponentInParent<PuzzleController>() && !interactor.GetComponent<PuzzleController>())
                    continue;

                // Set handle.
                interactor.OnTriggerEnter += HandleOnTriggerEnter;
                interactor.OnTriggerExit += HandleOnTriggerExit;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnTriggerEnter(Interactor interactor)
        {
            image.color = enabledColor;
            //LeanTween
            if (LeanTween.isTweening(gameObject))
                LeanTween.cancel(gameObject);

            transform.LeanScale(sizeAction, scaleTime);

        }
        void HandleOnTriggerExit(Interactor interactor)
        {
            image.color = disabledColor;

            if (LeanTween.isTweening(gameObject))
                LeanTween.cancel(gameObject);

            transform.LeanScale(sizeDefault, scaleTime);
        }
    }

}
