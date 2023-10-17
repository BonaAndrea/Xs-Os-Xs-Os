using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupManager : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    public UnityEvent OnFadeIn;
    public UnityEvent OnFadeOut;

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
            OnFadeIn.Invoke();
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
            OnFadeOut.Invoke();
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
