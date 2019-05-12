using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    public int BaseHealth { get; private set; }
    public float BaseSpeed { get; private set; }
    // Store any special attributew this enemy has
    string[] Attributes;

    public EnemyData(int _Health, float _Speed) {
        BaseHealth = _Health;
        BaseSpeed = _Speed;
    }

    public EnemyData(int _Health, float _Speed, string[] attri) {
        BaseHealth = _Health;
        BaseSpeed = _Speed;
        Attributes = attri;
    }


}
