using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingManager : MonoBehaviour
{
    [Header("?搾姣�?崚�")]
    public AudioMixer mixer;

    public void SetBGMVolume(float value)
    {
        mixer.SetFloat("BGM", value);
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFX", value);
    }
}
