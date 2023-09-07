using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupManager : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeGroup(true));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeGroup(false));
    }

    public IEnumerator FadeGroup(bool fadeIn, float durationInSeconds=0.25f)
    {
        if (fadeIn)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            float startAlpha = _canvasGroup.alpha;
            float targetAlpha = 1f;

            float elapsedTime = 0f;

            while (elapsedTime < durationInSeconds)
            {
                _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / durationInSeconds);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _canvasGroup.alpha = targetAlpha;
        }
        else
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            float startAlpha = _canvasGroup.alpha;
            float targetAlpha = 0f;

            float elapsedTime = 0f;

            while (elapsedTime < durationInSeconds)
            {
                _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / durationInSeconds);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _canvasGroup.alpha = targetAlpha;
        }
    }


    public void SetProperties(bool value)
    {
        if (value)
        {
            _canvasGroup.alpha = 1f;
        }
        else
        {
            _canvasGroup.alpha = 0f;
        }
        _canvasGroup.interactable = value;
        _canvasGroup.blocksRaycasts = value;
    }
    
}
