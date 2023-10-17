using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSourceController : MonoBehaviour
{
    public AudioSource Source;
    private bool _isSoundPlaying;
    private UnityEngine.UI.Button _button;


    private void Awake()
    {
        Source = GetComponent<AudioSource>();
        if (GetComponent<UnityEngine.UI.Button>())
        {
            _button = GetComponent<UnityEngine.UI.Button>();
        }
    }

    private void Update()
    {
        // Controlla se il suono è in riproduzione
        if (Source.isPlaying)
            _isSoundPlaying = true;
        else
            _isSoundPlaying = false;
    }

    public void PlaySound(AudioClip clip)
    {
        if (Source != null && !_isSoundPlaying)
        {
            Source.clip = clip;
            Source.Play();
        }

        if (_button != null)
        {
            DisableButtonInteractivity(clip);
        }
    }

    private void DisableButtonInteractivity(AudioClip clip)
    {
        UnityEngine.UI.Button button = GetComponent<UnityEngine.UI.Button>();
        if (button != null)
            button.interactable = false;

        Invoke("EnableButtonInteractivity", clip.length);
    }

    private void EnableButtonInteractivity()
    {
        UnityEngine.UI.Button button = GetComponent<UnityEngine.UI.Button>();
        if (button != null)
            button.interactable = true;
    }
}

