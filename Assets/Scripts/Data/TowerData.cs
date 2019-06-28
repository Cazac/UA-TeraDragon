using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTower", menuName = "Scriptable Objects/Tower Data")]
public class TowerData: ScriptableObject
{
    public int TowerLevel;
    public int BaseAttack;
    public float BaseRange;
    public float BaseAttackSpeed;
    public Sprite towerSprite;
    public ProjectileData projectile;
    // effects to give to its projectiles
    public string[] Effects;
    

}
