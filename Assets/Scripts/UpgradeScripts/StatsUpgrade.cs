using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewStatsUpgrade", menuName = "Scriptable Objects/Stats Upgrade")]
public class StatsUpgrade : UpgradeNodeTree {

    public float StatsMultiplier;
    public StatsCategory statsCategory;
    
}

public enum StatsCategory {
    TOWER_ATTACK_SPEED,
    TOWER_DAMAGE,
    TOWER_RANGE,
    ABILITY_TIMER,
    ABILITY_STATS,
    OTHER
}
