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


    public bool NextWaveCountdown()
    {
        timeUntilNextSpawn -= Time.deltaTime;

        if (timeUntilNextSpawn <= 0)
            return true;
        return false;
    }

    public bool WaveCountdown()
    {
        waveTimer -= Time.deltaTime;
        if (waveTimer <= 0)
        {
            Debug.Log("End of wave");
            return true;
        }

        return false;
    }
}
