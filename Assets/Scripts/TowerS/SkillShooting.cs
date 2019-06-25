using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShooting : MonoBehaviour
{
    [Header("Damage ???")]
    public int skillDamage = 20;

    [Header("Monsters In Range")]
    public List<EnemyScript> MonstersToShoot;

    [Header("Particule Parents")]
    public GameObject buildUpParticules;
    public GameObject releaseParticules;

    //////////////////////////////////////////////////////////

    private void Start()
    {
        MonstersToShoot = new List<EnemyScript>();
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
    public IEnumerator StartSkill()
    {
        buildUpParticules.SetActive(true);
        yield return new WaitForSeconds(3);
        releaseParticules.SetActive(true);

        //Hit all in range
        ShootAll();

        Destroy(gameObject);
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
                monster.TakeDamage(20);
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
