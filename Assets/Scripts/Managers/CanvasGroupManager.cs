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

    public IEnumerator FadeGroup(bool fadeIn)
    {
        yield return null;
        if (fadeIn)
        {
            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += 0.1f;
                yield return null;
            }

            _canvasGroup.interactable = true;
        }
        else
        {
            while (_canvasGroup.alpha >= 0f)
            {
                _canvasGroup.alpha -= 0.1f;
                yield return null;
            }

            _canvasGroup.interactable = false;
        }
    }
    
}
