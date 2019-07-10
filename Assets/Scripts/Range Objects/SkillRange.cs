using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRange : MonoBehaviour
{
    [Header("Skill Controller")]
    public SkillScript skillController;

    [Header("Particule Parents")]
    public GameObject backgroundParticules;
    public GameObject buildUpParticules;
    public GameObject releaseParticules;

    [Header("Monsters In Range")]
    public List<EnemyScript> MonstersToShoot;

    [Header("Skill Stats")]
    public float skillDuration = 0;
    public float skillDamage = 0;

    //////////////////////////////////////////////////////////

    private void Start()
    {
        MonstersToShoot = new List<EnemyScript>();

        //Build Data
        BuildSkill();

        //Start Skill
        StartCoroutine(StartSkill());
    }

    ///////////////
    /// <summary>
    /// UNDOCUMTNETED
    /// </summary>
    ///////////////
    public void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyScript monster = collider.gameObject.GetComponent<EnemyScript>();

        //Check if collider is a monster
        if (monster != null)
        {
            //Debug.Log("Tower Adding To List: " + collider.name);
            MonstersToShoot.Add(monster);
        }
    }

    ///////////////
    /// <summary>
    /// UNDOCUMTNETED
    /// </summary>
    ///////////////
    public void BuildSkill()
    {
        skillDuration = skillController.skillData.skillDuration;

        if (skillController.skillData.isDamageSingular)
        {
            skillDamage = skillController.skillData.singularDamage;
        }
        else if (skillController.skillData.isDamageConstant)
        {
            skillDamage = skillController.skillData.constantDamage;
        }

        //TO DO EFFECTS
        
    }

    ///////////////
    /// <summary>
    /// UNDOCUMTNETED
    /// </summary>
    ///////////////
    public IEnumerator StartSkill()
    {
        //Background
        if (backgroundParticules != null)
        {
            backgroundParticules.SetActive(true);
        }

        //Build Up
        if (buildUpParticules != null)
        {
            buildUpParticules.SetActive(true);
        }

        //Wait
        yield return new WaitForSeconds(3);

        //Release
        if (releaseParticules != null)
        {
            releaseParticules.SetActive(true);
        }

        //Hit all in range before destruction
        if (skillController.skillData.isDamageSingular)
        {
            ShootAll();
        }

        //Destroy Parent
        skillController.RemoveSkill();
    }

    ///////////////
    /// <summary>
    /// UNDOCUMTNETED
    /// </summary>
    ///////////////
    public void ShootAll()
    {
        int listMaxSizeCounter = MonstersToShoot.Count;


        while (listMaxSizeCounter > 0)
        {
            //Get Scripts
            EnemyScript monster = MonstersToShoot[listMaxSizeCounter - 1];

            if (monster != null)
            {
                //Deal that damage!
                monster.TakeDamage(skillDamage);
            }

            //Lower List
            listMaxSizeCounter--;
        }
    }

    ///////////////
    /// <summary>
    /// UNDOCUMTNETED
    /// </summary>
    ///////////////
    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != null)
        {
            //Remove From List
            MonstersToShoot.Remove(collider.gameObject.GetComponent<EnemyScript>());
        }
    }

}
