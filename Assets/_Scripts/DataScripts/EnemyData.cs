using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData : ScriptableObject {
    public int BaseHealth;
    public float BaseSpeed;
    // Store any special attributew this enemy has
    string[] Attributes;
    
}
