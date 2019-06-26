using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    
    public List<WorldTile> currentWaypoints;
    public List<List<WorldTile>> blockedWaypoints = new List<List<WorldTile>>();

    private int currentWaypoint = 0;
    private EnemyData enemyData;
    private TileNodes tileNodes;

    [Header("Enemy Stats")]
    public int MaxHealth;
    public int CurrentHealth;
    [Range(0.05f, 30f)]
    public float speed = 1.0f;

    // This seems better because allows us to change speed and reverse it
    Vector3 startPosition, endPosition, dir;

    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }


    /////////////////////////////////////////////////////////////////

    private void Start()
    {
        tileNodes = GameObject.FindObjectOfType<TileNodes>();

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
        if (currentWaypoints.Count > 1)
        {
            Move();
            DynamicPathRelocation(transform.position);
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

        StartPosition = currentWaypoints[currentWaypoint].transform.position;
        endPosition = currentWaypoints[currentWaypoint + 1].transform.position;

        dir = endPosition - StartPosition;

        transform.position += dir.normalized * speed * Time.fixedDeltaTime;

        if (Vector3.Distance(gameObject.transform.position, endPosition) < 0.5f)
        {
            if (currentWaypoint < currentWaypoints.Count - 2)
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


    /// <summary>
    /// Recalulate path when current path has a barrier
    /// <para>Function runs by scanning for path that contains blocked tile, 
    /// and redirect to path without blocked tile </para>
    /// </summary>
    /// <param name="currentPosition">Current captured position of enemy gameobject at runtime</param>
    private void DynamicPathRelocation(Vector3 currentPosition)
    {
        //scan map for block waypoibt
      
        //Reapply waypoints
        foreach (var path in tileNodes.pathData.paths)
        {
            //If there's still a path to take
            if (!tileNodes.pathData.blockedPaths.Contains(path) && tileNodes.pathData.blockedPaths.Count >= 1)
            {
                currentWaypoints = path;
                StartPosition = currentPosition;
                return;
            }

            //If there's still a path to take
            else
            {

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
