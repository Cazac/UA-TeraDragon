using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTower", menuName = "Scriptable Objects/Tower")]
public class TowerData: ScriptableObject
{
    [Header("Placement Tier")]
    public int TowerStartingLevel;
    public SoundObject shootingSFX;

    [Header("Tower Tier 1")]
    public Sprite towerSprite_T1;
    public ProjectileData projectile_T1;
    public float towerRange_T1;
    public float towerReloadSpeed_T1;

    [Header("Tower Tier 2")]
    public Sprite towerSprite_T2;
    public ProjectileData projectile_T2;
    public float towerRange_T2;
    public float towerReloadSpeed_T2;

    [Header("Tower Tier 3")]
    public Sprite towerSprite_T3;
    public ProjectileData projectile_T3;
    public float towerRange_T3;
    public float towerReloadSpeed_T3;
}
