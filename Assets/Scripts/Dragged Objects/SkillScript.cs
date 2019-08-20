using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScript : MonoBehaviour
{
    [Header("Refferences")]
    public SkillData skillData;
    public SkillRange skillRange;

    //////////////////////////////////////////////////////////

    public void RemoveSkill()
    {
        Destroy(gameObject);
    }

    //////////////////////////////////////////////////////////
}
