using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////
/// <summary>
///     
/// TowerShooting is used to find and shoot targets in range
/// 
/// </summary>
///////////////

public class TowerShooting : MonoBehaviour
{
    [Header("Projectile Prefab / Data")]
    //public GameObject FireProjectile;
    public ProjectileData projectilePresetData;

    [Header("Monsters In Range")]
    public List<EnemyScript> MonstersToShoot;

    [Header("Tower Stats")]
    public float timeToReload = 2f;
    public float reloadProgress = 0;
    public bool isReadyToShoot;
    public string TowerName;

    //////////////////////////////////////////////////////////

    private void Start()
    {
        MonstersToShoot = new List<EnemyScript>();
    }

    private void Update()
    {
        Reload();
        Shoot();
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
        else
        {
            //print("Not a Monsters");
        }
    }

    ///////////////
    /// <summary>
    /// UNDOCUMTNETED
    /// </summary>
    ///////////////
    public void Reload()
    {
        if (!isReadyToShoot)
        {
            reloadProgress += Time.deltaTime;
        }

        if (reloadProgress >= timeToReload)
        {
            reloadProgress = 0;
            isReadyToShoot = true;
        }
    }

    ///////////////
    /// <summary>
    /// UNDOCUMTNETED
    /// </summary>
    ///////////////
    public void Shoot()
    {
        if (!isReadyToShoot)
        {
            return;
        }
        else
        {
            if (MonstersToShoot.Count > 0)
            {

                GameObject monster_GO = MonstersToShoot[0].gameObject;

                GenerateProjectile(monster_GO);

                isReadyToShoot = false;
            }
        }
    }

    ///////////////
    /// <summary>
    /// Data to use for making a projectile is tower upgarade -> adding the sill tree -> adding any other bonuses. The "projectilePresetData" is reset to a new version on tower upgrade.
    /// </summary>
    ///////////////
    public void GenerateProjectile(GameObject monster)
    {
        GameObject projectile = Instantiate(projectilePresetData.projectilePrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        ProjectileFire projectileScript = projectile.GetComponent<ProjectileFire>();



        //Does Not get Modified
        projectileScript.isProjectile = projectilePresetData.isProjectile;

        //Add Skill tree values TO DO!!!!!
        projectileScript.projectileDamage = projectilePresetData.projectileDamage;
        projectileScript.projectileSpeed = projectilePresetData.projectileSpeed;
        projectileScript.projectileSlowdown = projectilePresetData.projectileSlowdown;

        //Does Not get Modified
        projectileScript.isExplosive = projectilePresetData.isExplosive;

        //Add Skill tree values TO DO!!!!!
        projectileScript.explosionRadius = projectilePresetData.explosionRadius;
        projectileScript.explosionLinger = projectilePresetData.explosionLinger;
        projectileScript.explosionDamage = projectilePresetData.explosionDamage;
        projectileScript.explosionSlowdown = projectilePresetData.explosionSlowdown;

        //Does Not get Modified
        projectileScript.isBeam = projectilePresetData.isProjectile;

        //Add Skill tree values TO DO!!!!!
        projectileScript.beamDamage = projectilePresetData.beamDamage;
        projectileScript.beamSlowdown = projectilePresetData.beamSlowdown;
        projectileScript.beamChainTargets = projectilePresetData.beamChainTargets;

        //Add enemy to track
        projectileScript.enemy = monster;
    }

    ///////////////
    /// <summary>
    /// UNDOCUMTNETED
    /// </summary>
    ///////////////
    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider == null)
        {
            print("ERROR");
        }
        else
        {
            //Remove From List
            MonstersToShoot.Remove(collider.gameObject.GetComponent<EnemyScript>());
        }
    }
}
