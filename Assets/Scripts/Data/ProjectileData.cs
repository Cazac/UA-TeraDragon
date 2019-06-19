using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Projectile")]
public class Projectile : ScriptableObject
{
    public string ProjectileName;
    public float BaseProjectileSpeed;
    public float BaseDamage;
    public Sprite sprite;
    // store any special 
    public string[] Effects;
}
