using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : ScriptableObject
{
    public ENEMY_STATUS status;
    public float countdown;
    // This will mean different things depending on the status
    public float statusEffect;
}

public enum ENEMY_STATUS
{
    COLD,
    FIRE,
}
