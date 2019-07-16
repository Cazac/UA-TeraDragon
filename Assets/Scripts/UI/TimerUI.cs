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
            waveText.text = "Next Waves Start in: 0";
            currentTimer.TimeUntilNextSpawn = 1;
        }
    }
}
