﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFire : MonoBehaviour
{
    //Who to chase
    public GameObject enemy;


    [Header("Projectile Data")]
    public bool isProjectile;
    public float projectileDamage;
    public float projectileSpeed;
    public float projectileSlowdown;

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
        }
    }


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
            float step = 100 * Time.deltaTime;

            //Look at?
            transform.up = target - transform.position;


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
            //print("Hit!");

            //Deal damage
            HitTarget();

            //Destroy Projectile
            Destroy(gameObject);
        }
    }


    public void HitTarget()
    {
        if (enemy == null)
        {
            //Remove projectile before hitting if there is nothing to hit
            Destroy(gameObject);
        }
        else
        {
            //Deal that damage!
            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
            enemyScript.TakeDamage(projectileDamage);
        }

        if (isExplosive)
        {
            //Target get a bounus explosion collition now



        }
    }

    public void BeamTarget()
    {
        if (enemy == null)
        {
            //Remove projectile before hitting if there is nothing to hit
            Destroy(gameObject);
        }
        else
        {
            //Deal that damage!
            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
            enemyScript.TakeDamage(projectileDamage);
        }

        if (isExplosive)
        {
            //Target get a bounus explosion collition now



        }
    }
}
