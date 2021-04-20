using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Zom.Pie.UI
{
    public class ActionButtonUI : MonoBehaviour
    {
        [SerializeField]
        bool changeColor = false;

        [SerializeField]
        Color32 enabledColor;

        [SerializeField]
        Color32 disabledColor;

        [SerializeField]
        bool scale = false;

        [SerializeField]
        float sizeActionScaleMultiplier = 1f;

        [SerializeField]
        bool changeSprite = false;

        [SerializeField]
        Sprite actionSprite;

        Image image;

        Vector3 sizeDefault;
        Vector3 sizeAction;
        float scaleTime = 0.2f;
        Sprite defaultSprite;


        void Awake()
        {
            image = GetComponent<Image>();

            if (changeColor)
            {
                image.color = disabledColor;
            }

            if (scale)
            {
                sizeDefault = transform.localScale;
                sizeAction = sizeActionScaleMultiplier * sizeDefault;
            }

            if (changeSprite)
            {
                defaultSprite = image.sprite;
            }
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
            if (changeColor)
            {
                image.color = enabledColor;
                //LeanTween
                if (LeanTween.isTweening(gameObject))
                    LeanTween.cancel(gameObject);
            }
            
            if(scale)
                transform.LeanScale(sizeAction, scaleTime);

            if (changeSprite)
                image.sprite = actionSprite;

        }
        void HandleOnTriggerExit(Interactor interactor)
        {
            if (changeColor)
            {
                image.color = disabledColor;

                if (LeanTween.isTweening(gameObject))
                    LeanTween.cancel(gameObject);
            }
            
            if(scale)
                transform.LeanScale(sizeDefault, scaleTime);

            if (changeSprite)
                image.sprite = defaultSprite;
        }
    }

}
