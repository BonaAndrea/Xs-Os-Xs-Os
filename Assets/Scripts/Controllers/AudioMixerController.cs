using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour
{
    public void ToggleAudioMixerEnabled(AudioMixer input)
    {
        float volume;
        input.GetFloat("volume", out volume);
        Debug.Log(volume);
        if (volume > -80.0f)
        {
            volume = -80.0f;
        }
        else
        {
            volume = 0.0f;
        }

        var result = input.SetFloat("volume", volume);
    }
    
}