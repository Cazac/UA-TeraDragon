using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundObject", menuName = "Scriptable Objects/Sound System")]
class SoundObject : ScriptableObject
{
    public string SoundName;
    public AudioClip AudioClip;
    public float Volume;
    public float Pitch;
    public Boolean IsAllowedLooping;
    public Boolean IsAllowedAutoDestruct;

    public SoundObject(string soundName, AudioClip audioClip, float volume, float pitch, bool isAllowedLooping, bool isAllowedAutoDestruct)
    {
        SoundName = soundName;
        AudioClip = audioClip;
        Volume = volume;
        Pitch = pitch;
        IsAllowedLooping = isAllowedLooping;
        IsAllowedAutoDestruct = isAllowedAutoDestruct;
    }

    public SoundObject()
    {
    }

    public void PlayClip(AudioSource audioSource)
    {
        audioSource.Play();
    }
}
