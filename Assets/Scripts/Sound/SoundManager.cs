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
    public bool IsMuteSoundtrack { get; set; }
    public bool IsMuteUI { get; set; }

    public bool ReturnControl { get => returnControl; set => returnControl = value; }


    public SoundObject[] soundClips;
    //private List<GameObject> uiGeneratedSounds = new List<GameObject>();

    private AudioClip currentClip;
    private bool triggerOnLevelLoad = false;
    private bool returnControl = true;

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            triggerOnLevelLoad = true;
        }
    }

    private void Update()
    {
        DontDestroyOnLoad(this);
        if (!mainAudioSourceSoundtrack.GetComponent<AudioSource>().isPlaying && returnControl)
        {
            LoopThroughSoundList(soundClips);
        }

        if (triggerOnLevelLoad)
        {
            LoopThroughSoundList(soundClips);
            triggerOnLevelLoad = false;
        }
    }

    public void PlaySpecificSound(String soundName)
    {
            foreach (var clip in soundClips)
            {
                if (clip.SoundName.Contains(soundName))
                    PlaySoundByName(clip);
            }
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

            else if (clip.SoundName.Contains("Main") && SceneManager.GetActiveScene().name.Contains("Main"))
                PlaySoundByName(clip);
        }
    }

    public void PlaySoundByName(SoundObject audioClip)
    {
        //Set default value from SoundObject
        mainAudioSourceSoundtrack.clip = audioClip.AudioClip;
        mainAudioSourceSoundtrack.pitch = audioClip.Pitch;
        mainAudioSourceSoundtrack.loop = true;
        mainAudioSourceSoundtrack.Play();


        //Begin lerping volume of sound
        if (audioClip.IsAllowedAudioDampening == true)
        {
            StartCoroutine(AudioVolumeDampeningOnLoad(mainAudioSourceSoundtrack, 0.5f, mainAudioSourceSoundtrack.volume, 0.2f));
        }
    }

    private IEnumerator AudioVolumeDampeningOnLoad(AudioSource audioSource, float smallestLerpValue, float initialVolumeValue, float lerpTime)
    {
        audioSource.volume = smallestLerpValue;

      

        while (audioSource.volume < initialVolumeValue)
        {
            if (audioSource.volume >= 0.98)
            {
                audioSource.volume = 1;
                Debug.Log("Coroutine stopped");
                StopCoroutine(AudioVolumeDampeningOnLoad(audioSource, smallestLerpValue, initialVolumeValue, lerpTime));
            }
            audioSource.volume += lerpTime * Time.deltaTime;
            yield return null;
        }

       
    }

    public void VolumeChangeSoundtrack(Slider slider)
    {
        mainAudioSourceSoundtrack.volume = slider.value;
    }

    public void VolumeChangeUI(Slider slider)
    {
        mainAudioSourceUI.volume = slider.value;
    }

    public void MuteUI()
    {
        if (IsMuteUI)
        {
            IsMuteUI = false;
        }

        else
        {
            IsMuteUI = true;
        }
        mainAudioSourceUI.mute = IsMuteUI;
    }

    public void MuteSoundtrack()
    {

        if (IsMuteSoundtrack)
        {
            IsMuteSoundtrack = false;
        }

        else
        {
            IsMuteSoundtrack = true;
        }
        mainAudioSourceSoundtrack.mute = IsMuteSoundtrack;
    }


}

