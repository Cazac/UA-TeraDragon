using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class SoundManager : MonoBehaviour
{
    public AudioSource mainAudioSource;
    public SoundObject[] soundClips;
    private List<GameObject> uiGeneratedSounds = new List<GameObject>();

    private void Update()
    {
        if (!GameObject.FindObjectOfType<AudioSource>().isPlaying)
            LoopThroughSoundList(soundClips);
    }

    public void PlayOnUIClick(SoundObject clip)
    {
        GameObject audioEvent = new GameObject("Audio Event_" + clip.SoundName);
        audioEvent.AddComponent<AudioSource>().clip = clip.AudioClip;
        audioEvent.GetComponent<AudioSource>().Play();
        AutoDestruct autoDestruct = audioEvent.AddComponent<AutoDestruct>();
        
        uiGeneratedSounds.Add(audioEvent);
     }

    private void LoopThroughSoundList(SoundObject[] clips)
    {
        foreach (var clip in clips)
        {
            if (clip.SoundName.Contains("Menu") && SceneManager.GetActiveScene().name.Contains("Menu"))
                PlaySoundByName(clip);

            if (clip.SoundName.Contains("MainGame") && SceneManager.GetActiveScene().name.Contains("MainGame"))
                PlaySoundByName(clip);
        }
    }

    public void PlaySoundByName(SoundObject audioClip)
    {
       mainAudioSource.clip = audioClip.AudioClip;
       mainAudioSource.volume = audioClip.Volume;
       mainAudioSource.pitch = audioClip.Pitch;

       mainAudioSource.loop = true;
       mainAudioSource.Play();
    }
}

