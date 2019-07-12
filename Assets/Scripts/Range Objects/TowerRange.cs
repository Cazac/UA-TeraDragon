using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////
/// <summary>
///     
/// TowerShooting is used to find and shoot targets in range of the tower range raduis Gameobject
/// 
/// </summary>
///////////////

public class TowerRange : MonoBehaviour
{
    [Header("Tower Controller")]
    public TowerScript parentTowerScript;

    [Header("Line Renderer")]
    public LineRenderer lineMaker;

    [Header("Projectile Scriptable Data")]
    public ProjectileData currentProjectileData;

    [Header("Monsters In Range")]
    public List<EnemyScript> MonstersToShoot;

    [Header("Projectiles Fired")]
    public List<ProjectileFire> projectilesShot;

    [Header("Tower Stats")]
    public float timeToReload = 0;
    public float reloadProgress = 0;
    public bool isReadyToShoot = false;

    //////////////////////////////////////////////////////////

    private void Start()
    {
        MonstersToShoot = new List<EnemyScript>();
        projectilesShot = new List<ProjectileFire>();
        GetTowerData();
    }

    private void Update()
    {
        //Reload
        Reload();

        //Shoot
        if (currentProjectileData.isProjectile)
        {
            Shoot();
        }
    }

    //////////////////////////////////////////////////////////

    ///////////////
    /// <summary>
    /// Update the tower data from the tower controller script
    /// </summary>
    ///////////////
    public void GetTowerData()
    {
        int towerTier = parentTowerScript.currentTowerTier;
        float scale;

        switch (towerTier)
        {
            case 0:

                Debug.Log("Error");

                break;

            case 1:

                //New Reload Speed
                timeToReload = parentTowerScript.towerData.towerReloadSpeed_T1;

                //New Range Scale Size
                scale = parentTowerScript.towerData.towerRange_T1;
                gameObject.transform.localScale = new Vector3(scale, scale, scale);

                //New Projectile
                currentProjectileData = parentTowerScript.towerData.projectile_T1;

                break;

            case 2:

                //New Reload Speed
                timeToReload = parentTowerScript.towerData.towerReloadSpeed_T2;

                //New Range Scale Size
                scale = parentTowerScript.towerData.towerRange_T2;
                gameObject.transform.localScale = new Vector3(scale, scale, scale);

                //New Projectile
                currentProjectileData = parentTowerScript.towerData.projectile_T3;

                break;

            case 3:

                //New Reload Speed
                timeToReload = parentTowerScript.towerData.towerReloadSpeed_T3;

                //New Range Scale Size
                scale = parentTowerScript.towerData.towerRange_T3;
                gameObject.transform.localScale = new Vector3(scale, scale, scale);

                //New Projectile
                currentProjectileData = parentTowerScript.towerData.projectile_T3;

                break;
        }
    }

    ///////////////
    /// <summary>
    /// When a monster collider enters the range of the tower add it to the shooting list
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

            if (currentProjectileData.isBeam)
            {
                Beam();
            }
        }
    }

    ///////////////
    /// <summary>
    /// Use time scale to get closer to having a full reload and then setting isReadyToShoot to true
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
    /// Check if able to shoot and avalible monster then generate a projectile
    /// </summary>
    ///////////////
    public void Shoot()
    {
        if (isReadyToShoot)
        {
            if (MonstersToShoot.Count > 0)
            {
                //Find first monsters
                GameObject monster_GO = MonstersToShoot[0].gameObject;

                //Generate Projectile with target
                GenerateProjectile(monster_GO);

                //reset shooting value
                isReadyToShoot = false;
            }
        }
    }

    ///////////////
    /// <summary>
    /// Check if able to shoot and avalible monster then generate a projectile
    /// </summary>
    ///////////////
    public void Beam()
    {


        ////?????????????????
        int maxMonsterCount = currentProjectileData.beamChainTargets;




        if (MonstersToShoot.Count > 0)
        {
            //Find first monsters
            GameObject monster_GO = MonstersToShoot[0].gameObject;

            //Generate Projectile with target
            GenerateProjectile(monster_GO);

            //Generate Projectile with target
            LinkBeam(monster_GO);
        }
        
    }


    public void BeamDamage()
    {






    }


    public void LinkBeam(GameObject monster)
    {






    }

    ///////////////
    /// <summary>
    /// Data to use for making a projectile is tower upgarade -> adding the sill tree -> adding any other bonuses. The "projectilePresetData" is reset to a new version on tower upgrade.
    /// </summary>
    ///////////////
    public void GenerateProjectile(GameObject monster)
    {
        GameObject projectile = Instantiate(currentProjectileData.projectilePrefab, parentTowerScript.firingPoint.transform.position, Quaternion.identity, parentTowerScript.firingPoint.transform);
        ProjectileFire projectileScript = projectile.GetComponent<ProjectileFire>();

        //Get Tower
        projectileScript.tower = gameObject;

        //Does Not get Modified
        projectileScript.isProjectile = currentProjectileData.isProjectile;
        projectileScript.isExplosive = currentProjectileData.isExplosive;
        projectileScript.isBeam = currentProjectileData.isBeam;

        //Add Skill tree values TO DO!!!!!
        projectileScript.projectileDamage = currentProjectileData.projectileDamage;
        projectileScript.projectileSpeed = currentProjectileData.projectileSpeed;
        projectileScript.projectileSlowdown = currentProjectileData.projectileSlowdown;
        projectileScript.projectileSlowdownTime = currentProjectileData.projectileSlowdownTime;


        //Add Skill tree values TO DO!!!!!
        projectileScript.explosionRadius = currentProjectileData.explosionRadius;
        projectileScript.explosionLinger = currentProjectileData.explosionLinger;
        projectileScript.explosionDamage = currentProjectileData.explosionDamage;
        projectileScript.explosionSlowdown = currentProjectileData.explosionSlowdown;


        //Add Skill tree values TO DO!!!!!
        projectileScript.beamDamage = currentProjectileData.beamDamage;
        projectileScript.beamReload = currentProjectileData.beamReload;
        projectileScript.beamChainTargets = currentProjectileData.beamChainTargets;

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
        if (collider != null)
        {
            //Remove From List
            MonstersToShoot.Remove(collider.gameObject.GetComponent<EnemyScript>());

            if (currentProjectileData.isBeam)
            {

                //Tell enemy stop
                collider.gameObject.GetComponent<EnemyScript>()

                //tell prjectile to fuck off


                //projectilesShot.Find
                //collider.GetComponent<EnemyScript>().;
            }
        }
    }

    //////////////////////////////////////////////////////////
}
