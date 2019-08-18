using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFire : MonoBehaviour
{
    [Header("Who to chase")]
    public GameObject enemy;

    [Header("Tower That Shot")]
    public GameObject tower;

    [Header("Projectile Data")]
    public bool isProjectile;
    public int projectileDamage;
    public float projectileSpeed;
    public float projectileSlowdown;
    public float projectileSlowdownTime;

    [Header("Bonus Explosive Data")]
    public bool isExplosive;
    public float explosionRadius;
    public float explosionLinger;
    public int explosionDamage;
    public float explosionSlowdown;

    [Header("Beam Data")]
    public bool isBeam;
    public int beamDamage;
    public float beamReload;
    public int beamChainTargets;

    [Header("Reload")]
    public float beamReloadCurrent;

    //////////////////////////////////////////////////////////

    void Start()
    {
        
    }

    void Update()
    {
        if (isProjectile)
        {
            //Move position closer for collision
            ChaseTarget();
        }
        else if (isBeam)
        {
            //Latch
            BeamTargetTrace();
            BeamReload();
            BeamDamage();
        }
    }

    //////////////////////////////////////////////////////////


    ///////////////
    /// <summary>
    /// UNDOCUMTNETED
    /// </summary>
    ///////////////
    public void ChaseTarget()
    {
        if (enemy == null)
        {
            //Remove projectile before chasing if there is nothing to chase
            Destroy(gameObject);
        }
        else
        {
            Vector3 target = enemy.transform.position;

            //USE A SPEED VALUE
            float step = projectileSpeed * Time.deltaTime;

            //Look at?
            transform.right = target - transform.position;


            transform.position = Vector2.MoveTowards(gameObject.transform.position, target, step);
        }
    }

    ///////////////
    /// <summary>
    /// UNDOCUMTNETED
    /// </summary>
    ///////////////
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == enemy)
        {
            //Deal damage
            HitTarget();

            //Destroy Projectile
            Destroy(gameObject);
        }
    }
    
    ///////////////
    /// <summary>
    /// UNDOCUMTNETED
    /// </summary>
    ///////////////
    public void HitTarget()
    {
        if (enemy == null)
        {
            //Remove projectile before hitting if there is nothing to hit
            Destroy(gameObject);
        }
        else
        {
            //Get The Monster!
            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();

            //Slowdown!
            if (projectileSlowdown > 0)
            {
                enemyScript.AddSlow(projectileSlowdown, projectileSlowdownTime);
            }

            //Deal that damage!
            enemyScript.TakeDamage(projectileDamage);
        }

        //Bonus Damage!
        if (isExplosive)
        {
            //Target get a bounus explosion collition now



        }
    }

    //////////////////////////////////////////////////////////

    public void BeamTargetTrace()
    {
        if (enemy == null)
        {
            //Remove projectile before hitting if there is nothing to hit
            Destroy(gameObject);
        }
        else
        {
            //Get The Monster!
            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
            LineRenderer lineMaker = tower.GetComponent<LineRenderer>();

            //Get Positions!
            Vector3 origin = gameObject.transform.position;
            Vector3 destintation = enemyScript.transform.position;

            //Edit Z Values!
            origin.z = -11f;
            destintation.z = -11f;

            int monstersToBeShotCount = beamChainTargets;

        

            //Draw Lines!
            lineMaker.SetPosition(0, origin);
            lineMaker.SetPosition(1, destintation);
        }
    }

    public void BeamReload()
    {

    }

    public void BeamDamage()
    {
        if (enemy == null)
        {
            //Remove projectile before hitting if there is nothing to hit
            Destroy(gameObject);
        }
        else
        {
            //Get The Monster!
            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();

            //Deal that damage!
            enemyScript.TakeDamage(beamDamage);
        }
    }

    //////////////////////////////////////////////////////////
}
