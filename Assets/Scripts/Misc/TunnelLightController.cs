using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Zom.Pie
{
    public class TunnelLightController : MonoBehaviour
    {
        Bloom bloom;

        // Start is called before the first frame update
        void Start()
        {
            SetBloom();

            StartCoroutine(PlayEffect());
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetBloom()
        {
            Volume volume = GameObject.FindObjectOfType<Volume>();
            bloom = null;
            volume.profile.TryGet(out bloom);
             
        }

        IEnumerator PlayEffect()
        {
            float delay = 0;
            yield return new WaitForSeconds(delay);

            float time = 30f - delay;
            float targetIntensity = 12000f;
            float targetThreshold = 0f;

            float intensityDefault = 0, thresholdDefault = 0;

            LeanTween.value(gameObject, OnIntensityUpdate, intensityDefault, targetIntensity, time).setEaseInExpo();
            LeanTween.value(gameObject, OnThresholdUpdate, thresholdDefault, targetThreshold, time).setEaseInExpo();
        }

        void OnIntensityUpdate(float value)
        {
            bloom.intensity.value = value;
        }


        void OnThresholdUpdate(float value)
        {
            bloom.threshold.value = value;
        }

    }


}
