using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer
{
    private float timeUntilNextSpawn;
    private float waveTimer;
    private float spawnRatePerSecond;

    private TimerUI timerUI;

    //////////////////////////////////////////////////////////

    public float TimeUntilNextSpawn
    {
        get
        {
            return (int)Mathf.RoundToInt(timeUntilNextSpawn);
        }
        set => timeUntilNextSpawn = value;
    }
    public Timer(float timeUntilSpawn, float waveTimer)
    {
        this.timeUntilNextSpawn = timeUntilSpawn;
        this.waveTimer = waveTimer;
    }
    public float SpawnRatePerSecond { get => SpawnRatePerSecond; set => SpawnRatePerSecond = value; }
    public float WaveTimer
    {
        get
        {
            return (int)Mathf.RoundToInt(waveTimer);
        }
        set => waveTimer = value;
    }

    //////////////////////////////////////////////////////////

    /// <summary>
    ///Constructor for creating timer for current wave
    /// </summary>
    /// <param name="timeUntilSpawn">Time until next wave</param>
    /// <param name="waveTimer">Time for current wave</param>
    /// <param name="spawnRatePerSecond">How many enemy can be spawn per second</param>
    public Timer(float timeUntilSpawn, float waveTimer, float spawnRatePerSecond)
    {
        TimeUntilNextSpawn = timeUntilSpawn;
        WaveTimer = waveTimer;
        SpawnRatePerSecond = spawnRatePerSecond;
    }

    /// <summary>
    ///Countdown timer between wave when a wave is finished
    /// </summary>
    /// <returns>Return true when timer hits 0</returns> 
    public bool NextWaveCountdown()
    {
        //Set Reff
        timerUI = GameObject.FindObjectOfType<TimerUI>();

        //Subtract Time
        timeUntilNextSpawn -= Time.deltaTime;

        //Update UI
        timerUI.UpdateInterwaveTimer(timeUntilNextSpawn, this);

        //Check if timer is done
        if (timeUntilNextSpawn <= 0)
        {
            Debug.Log("Start New Wave");
            return true;
        }

        return false;
    }

    /// <summary>
    ///Countdown timer for a wave
    /// </summary>
    /// <returns>Return true when timer hits 0</returns> 
    public bool WaveCountdown()
    {
        //Set Reff
        timerUI = GameObject.FindObjectOfType<TimerUI>();

        //Subtract Time
        waveTimer -= Time.deltaTime;

        //Update UI + Send Timer
        timerUI.UpdateWaveTimer(waveTimer, this);

        //Check if timer is done
        if (waveTimer <= 0)
        {
            GameObject.FindObjectOfType<PlayerStats>().IncrementWaveSprite();
            PlayerStats playerstats = GameObject.FindObjectOfType<PlayerStats>();

            if (playerstats.currentWaveCounter % 2 == 0)
            {
                //Add a miner
                playerstats.minersOwned++;
            }
  

            return true;
        }

        return false;
    }


}
