using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    EnemyData enemyData;
    public List<WorldTile> waypoints;
    private int currentWaypoint = 0;


    [Header("Enemy Stats")]
    public int MaxHealth;
    public int CurrentHealth;
    [Range(0.05f, 30f)]
    public float speed = 1.0f;

    // This seems better because allows us to change speed and reverse it
    Vector3 startPosition, endPosition, dir;
  

    /////////////////////////////////////////////////////////////////

    private void Start()
    {
        if (enemyData != null)
        {
            // TO-DO: need to add modifyier calculations
            speed = enemyData.BaseSpeed;
            Debug.Log(enemyData.name + " has spawned");
        }
        else
        {
            Debug.Log("Spawing Monster from prefab data");
        }
    }

    private void FixedUpdate()
    {
        if (waypoints.Count > 1)
        {
            Move();
        }
    }

    /////////////////////////////////////////////////////////////////

    ///////////////
    /// <summary>
    /// UNDOCUMENTED
    /// </summary>
    ///////////////
    private void Move()
    {

        startPosition = waypoints[currentWaypoint].transform.position;
        endPosition = waypoints[currentWaypoint + 1].transform.position;

        dir = endPosition - startPosition;

        transform.position += dir.normalized * speed * Time.fixedDeltaTime;

        if (Vector3.Distance(gameObject.transform.position, endPosition) < 0.5f)
        {
            if (currentWaypoint < waypoints.Count - 2)
            {
                currentWaypoint++;
                // TODO: Rotate into move direction
            }
            else
            {
                // to be replaced with more complete function

                if (enemyData != null)
                {
                    Debug.Log(enemyData.name + " has died");
                }
                else
                {
                    Debug.Log("Death???");
                }

                Destroy(gameObject);


            }
        }

    }

    ///////////////
    /// <summary>
    /// UNDOCUMENTED
    /// </summary>
    ///////////////
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check for collision with the ending tile
        if (collision.transform.GetComponent<BaseNode>() != null)
        {
            collision.transform.GetComponent<BaseNode>().BaseIsHit(1);
            Destroy(gameObject);
        }
    }


    public void TakeDamage(float damage)
    {
        //Normalize float vs int???
        CurrentHealth -= (int)damage;

        //Armor??????

        if (CurrentHealth <= 0)
        {
            print("Death takes me...");
            Destroy(gameObject);
        }
    }

    //To DO
    public void ApplySlow()
    {

    }
}
