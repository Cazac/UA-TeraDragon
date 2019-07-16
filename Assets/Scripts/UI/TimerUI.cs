using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [Header("Text")]
    public Text waveText;
    public Button skipWaveButton;

    public void UpdateWaveTimer(float time)
    {
        int timeLeft = (int)time;
        waveText.text = "Wave Lasts For: " + timeLeft;
    }

    public void UpdateInterwaveTimer(float time)
    {
        int timeLeft = (int)time;
        waveText.text = "Next Waves Start in: " + timeLeft;
    }

}
