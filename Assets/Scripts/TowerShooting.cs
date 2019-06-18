using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShooting : MonoBehaviour
{


    public List<EnemyScript> MonstersToShoot;

    [Header("Projectile Prefabs")]
    public GameObject FireProjectile;


    public float timeToReload = 2;
    public float reloadProgress = 0;

    public bool isReadyToShoot;

    void Start()
    {
        MonstersToShoot = new List<EnemyScript>();
    }

    // Update is called once per frame
    private void Update()
    {


        Reload();
        Shoot();
 


    }


    public void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Tower Adding To List: " + collider.name);

        //Check if Monster
        //Add TO shoot list

        EnemyScript monster = collider.gameObject.GetComponent<EnemyScript>();


        if (monster != null)
        {
            MonstersToShoot.Add(monster);
        }
        else
        {
            //print("Not a MOnsters");
        }


    }


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

                print("Shoot!");

                GameObject monster_GO = MonstersToShoot[0].gameObject;

                GenerateProjectile(monster_GO);

                isReadyToShoot = false;
            }
        }


    }


    public void GenerateProjectile(GameObject monster)
    {
        GameObject projectile = Instantiate(FireProjectile, gameObject.transform.position, Quaternion.identity, gameObject.transform);


        ProjectileFire projectileScript = projectile.GetComponent<ProjectileFire>();

        projectileScript.enemy = monster;


    }



    public void OnTriggerStay2D()
    {
        // ???
    }


    public void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log("Tower Removing From List: " + collider.name);
        //Remove From List


        if (collider == null)
        {
            print("ERROR");
        }

        MonstersToShoot.Remove(collider.gameObject.GetComponent<EnemyScript>());

    }



}
