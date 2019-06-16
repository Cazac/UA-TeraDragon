using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class SoundManager : MonoBehaviour
{
    public AudioSource mainAudioSourceSoundtrack;
    public AudioSource mainAudioSourceUI;

    public SoundObject[] soundClips;
    //private List<GameObject> uiGeneratedSounds = new List<GameObject>();

    private AudioClip currentClip;

    private void Update()
    {
        DontDestroyOnLoad(this);
        if (!mainAudioSourceSoundtrack.GetComponent<AudioSource>().isPlaying)
            LoopThroughSoundList(soundClips);
    }

    public void PlayOnUIClick(SoundObject clip)
    {
        if (mainAudioSourceUI.GetComponent<AudioSource>().isPlaying)
            mainAudioSourceUI.clip = null;
        mainAudioSourceUI.clip = clip.AudioClip;

        mainAudioSourceUI.Play();
    }

    private void LoopThroughSoundList(SoundObject[] clips)
    {
        foreach (var clip in clips)
        {
            Debug.Log("Looping");
            if (clip.SoundName.Contains("Menu") && SceneManager.GetActiveScene().name.Contains("Menu"))
            {
                PlaySoundByName(clip);
            }

            else if (clip.SoundName.Contains("MainGame") && SceneManager.GetActiveScene().name.Contains("MainGame"))
                PlaySoundByName(clip);
        }
    }

    public void PlaySoundByName(SoundObject audioClip)
    {
       mainAudioSourceSoundtrack.clip = audioClip.AudioClip;
       mainAudioSourceSoundtrack.volume = audioClip.Volume;
       mainAudioSourceSoundtrack.pitch = audioClip.Pitch;

       mainAudioSourceSoundtrack.loop = true;
       mainAudioSourceSoundtrack.Play();
    }

    public void VolumeChangeSoundtrack(Slider slider)
    {
        mainAudioSourceSoundtrack.volume = slider.value;
    }

    public void VolumeChangeUI(Slider slider)
    {
        mainAudioSourceUI.volume = slider.value;
    }

}

