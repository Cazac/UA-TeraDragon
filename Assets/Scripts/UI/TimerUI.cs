using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [Header("Text")]
    public Text waveText;
    public Button skipWaveButton;
    public Timer currentTimer;

    public void UpdateWaveTimer(float time, Timer timer)
    {
        int timeLeft = (int)time;

        //MUSIC (Battle)
        if (!(FindObjectOfType<SoundManager>().mainAudioSourceSoundtrack.clip.name == "Landing"))
        {
            if (!(FindObjectOfType<SoundManager>().mainAudioSourceSoundtrack.clip.name == "Death on a Distant World"))
            {
                //Debug.Log("Battle");
                FindObjectOfType<SoundManager>().PlaySpecificSound("Main");
            }
        }

        if (timeLeft != 0)
        {
            waveText.text = "Wave Lasts For: " + timeLeft;
        }
        else
        {
            waveText.text = "Wave Over";
        }

        //Set New Timer
        currentTimer = timer;

        //Disable Button
        skipWaveButton.interactable = false;
    }

    public void UpdateInterwaveTimer(float time, Timer timer)
    {
        //MUSIC (Interwave)
        if (!(FindObjectOfType<SoundManager>().mainAudioSourceSoundtrack.clip.name == "Calm"))
        {
            if (!(FindObjectOfType<SoundManager>().mainAudioSourceSoundtrack.clip.name == "Death on a Distant World"))
            {
                //Debug.Log("Inter");
                FindObjectOfType<SoundManager>().PlaySpecificSound("Inter");
            }
        }

        int timeLeft = (int)time;
        waveText.text = "Next Waves Start in: " + timeLeft;

        //Set New Timer
        currentTimer = timer;

        //Enable Button
        skipWaveButton.interactable = true;
    }

    public void SkipInterwave()
    {
        if (currentTimer != null)
        {
            //Give Gems to Miners
            GameObject minerParent = GameObject.Find("Miner Parent");

            foreach (Transform child in minerParent.transform)
            {
                MinerScript minerScript = child.gameObject.GetComponent<MinerScript>();

                minerScript.timer += currentTimer.TimeUntilNextSpawn;
            }

            PlayerStats playerStats = FindObjectOfType<PlayerStats>();

            playerStats.skillCountdown_Red += currentTimer.TimeUntilNextSpawn;
            playerStats.skillCountdown_Blue += currentTimer.TimeUntilNextSpawn;
            playerStats.skillCountdown_Green += currentTimer.TimeUntilNextSpawn;
            playerStats.skillCountdown_Yellow += currentTimer.TimeUntilNextSpawn;




            waveText.text = "Next Waves Start in: 0";
            currentTimer.TimeUntilNextSpawn = 1;


    
        }
    }
}
