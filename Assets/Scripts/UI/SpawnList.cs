using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnList : ICloneable
{
    public WaveData wave;
    public List<WorldTile> spawnTileList;

    public SpawnList(WaveData wave, List<WorldTile> spawnTileList)
    {
        this.wave = wave;
        this.spawnTileList = spawnTileList;
    }
    public SpawnList()
    {

    }


    /// <summary>
    /// Enable cloning to circumvent node being deleted on start
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
        return MemberwiseClone();
    }
}

