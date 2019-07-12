using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Scriptable Objects/Projectile")]
public class ProjectileData : ScriptableObject
{
    [Header("Prefab")]
    public GameObject projectilePrefab;

    [Header("Projectile Data")]
    public bool isProjectile;
    public float projectileDamage;
    public float projectileSpeed;
    public float projectileSlowdown;
    public float projectileSlowdownTime;
    public float projectilePoison;
    public float projectilePoisonTime;

    [Header("Bonus Explosive Data")]
    public bool isExplosive;
    public float explosionRadius;
    public float explosionLinger;
    public float explosionDamage;
    public float explosionSlowdown;

    [Header("Beam Data")]
    public bool isBeam;
    public float beamDamage;
    public float beamSlowdown;
    public int beamChainTargets;
}