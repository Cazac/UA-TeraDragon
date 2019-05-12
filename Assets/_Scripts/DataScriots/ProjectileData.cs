using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileData
{
    public string ProjectileName { get; private set; }
    public float BaseProjectileSpeed { get; private set; }
    public float BaseDamage { get; private set; }
    // store any special 
    public string[] Effects;

    public ProjectileData(string _name, float _proj, float _baseDamage) {
        ProjectileName = _name;
        BaseProjectileSpeed = _proj;
        BaseDamage = _baseDamage;
    }

    public ProjectileData(string _name, float _proj, float _baseDamage, string[] _effects) {
        ProjectileName = _name;
        BaseProjectileSpeed = _proj;
        BaseDamage = _baseDamage;
        Effects = _effects;
    }
}
