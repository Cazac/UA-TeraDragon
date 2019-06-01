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
    public string[] attributes;

    // Start is called before the first frame update
    void Start()
    {
        if (enemyData != null)
        {
            // TO-DO: need to add modifyier calculations
            speed = enemyData.BaseSpeed;
        }
        if (enemyData != null)
         Debug.Log(enemyData.name + " has spawned");
    }


    private void FixedUpdate()
    {

        Move();
    }


    Vector3 startPosition, endPosition, dir;
    // This seems better because allows us to change speed and reverse it
    void Move()
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
                    Debug.Log(enemyData.name + " has died");
                Destroy(gameObject);

            }
        }

    }



}
