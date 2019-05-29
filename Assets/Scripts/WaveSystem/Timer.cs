using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float timeUntilNextSpawn;
    private float waveTimer;

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

    private float spawnRatePerSecond; 
    public float SpawnRatePerSecond { get => SpawnRatePerSecond; set => SpawnRatePerSecond = value; }


    public Timer(float timeUntilSpawn, float waveTimer, float spawnRatePerSecond) 
    {
        TimeUntilNextSpawn = timeUntilSpawn;
        WaveTimer = waveTimer;
        SpawnRatePerSecond = spawnRatePerSecond;
    }

    public float WaveTimer 
    {
         get
         {
             return (int)Mathf.RoundToInt(waveTimer);
         }
         set => waveTimer = value; 
    }


    public bool NextWaveCountdown(float setTime)
    {
        timeUntilNextSpawn = setTime;
        timeUntilNextSpawn -= Time.deltaTime;

        if (timeUntilNextSpawn <= TimeUntilNextSpawn)
            return true;
        return false;
    }

    public bool WaveCountdown(float setTime)
    {
        waveTimer = setTime;
        waveTimer -= Time.deltaTime;
        if (waveTimer <= TimeUntilNextSpawn)
            return true;
        return false;
    }
}
