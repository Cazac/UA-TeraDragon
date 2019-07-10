using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Scriptable Objects/Skill")]
public class SkillData : ScriptableObject
{
    [Header("Skill Data")]
    public float skillDuration;

    [Header("Singular Damage")]
    public bool isDamageSingular;
    public float singularDamage;

    [Header("Constant Damage")]
    public bool isDamageConstant;
    public float constantDamage;

    [Header("Effects")]
    public float slowdown;
    public float poison;
}
