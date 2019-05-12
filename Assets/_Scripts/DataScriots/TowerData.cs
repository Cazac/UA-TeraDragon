using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData
{
    public string TowerName { get; private set; }
    public int TowerLevel { get; private set; }
    public int BaseAttack { get; private set; }
    public float BaseRange { get; private set; }
    public float BaseAttackSpeed { get; private set; }
    // effects to give to its projectiles
    public string[] Effects;

    public TowerData(string _towerName, int _towerLevel, int _baseAtt, int _baseRange, int _baseAttSpd) {
        TowerName = _towerName;
        TowerLevel = _towerLevel;
        BaseAttack = _baseAtt;
        BaseRange = _baseRange;
        BaseAttackSpeed = _baseAttSpd;
    }

    public TowerData(string _towerName, int _towerLevel, int _baseAtt, int _baseRange, int _baseAttSpd, string[] _effects) {
        TowerName = _towerName;
        TowerLevel = _towerLevel;
        BaseAttack = _baseAtt;
        BaseRange = _baseRange;
        BaseAttackSpeed = _baseAttSpd;
        Effects = _effects;
    }

}
