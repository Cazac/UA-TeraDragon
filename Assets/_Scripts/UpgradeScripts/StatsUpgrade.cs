using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewStatsUpgrade", menuName = "ScriptableObjects/Stats Upgrade")]
public class StatsUpgrade : UpgradeNode {

    public float StatsMultiplier;
    public StatsCategory statsCategory;
    
}

public enum StatsCategory {
    ATTACK_SPEED,
    DAMAGE,
    RANGE,
    OTHER
}
