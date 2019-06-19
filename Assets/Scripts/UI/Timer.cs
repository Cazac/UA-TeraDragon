using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float timeUntilNextSpawn;
    private float waveTimer;
    private float spawnRatePerSecond;

    //------------------------Properties------------------------
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
    //------------------------Properties------------------------


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
        timeUntilNextSpawn -= Time.deltaTime;

        if (timeUntilNextSpawn <= 0)
            return true;
        return false;

    }

    /// <summary>
    ///Countdown timer for a wave
    /// </summary>
    /// <returns>Return true when timer hits 0</returns> 
    public bool WaveCountdown()
    {
        waveTimer -= Time.deltaTime;
        if (waveTimer <= 0)
        {
            //Debug.Log("End of wave");
            return true;
        }

        return false;
    }
}
