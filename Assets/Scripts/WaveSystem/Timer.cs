using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public int TimeUntilSpawn { get; set; }
    public int WaveTimer { get; set; }

    private float internalTimer;

    public bool SpawnCountdown()
    {
        internalTimer += Time.deltaTime;
        if(internalTimer >= TimeUntilSpawn)
            return true;
        return false;
    }

    public bool WaveCountdown()
    {
        internalTimer += Time.deltaTime;
        if(internalTimer >= TimeUntilSpawn)
            return true;
        return false;
    }
}
