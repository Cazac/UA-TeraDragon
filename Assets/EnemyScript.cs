using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    EnemyData enemyData;

    // Start is called before the first frame update
    void Start()
    {
        lastWaypointSwitchTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void FixedUpdate() {
        
    }
    

    // This seems better because allows us to change speed and reverse it
    void Move() {
        Vector3 startPosition = waypoints[currentWaypoint].transform.position;
        Vector3 endPosition = waypoints[currentWaypoint + 1].transform.position;

        Vector3 dir = waypoints[currentWaypoint + 1].transform.position - waypoints[currentWaypoint].transform.position;

        transform.position += dir.normalized * speed * Time.deltaTime;

        if (Vector3.Distance(gameObject.transform.position,endPosition) < 1f) {
            if (currentWaypoint < waypoints.Count - 2) {

                currentWaypoint++;
                lastWaypointSwitchTime = Time.time;
                // TODO: Rotate into move direction
            }
            else {
                // to be replaced with more complete function
                Destroy(gameObject);

            }
        }

    }

    // Copying tutorial: https://www.raywenderlich.com/269-how-to-create-a-tower-defense-game-in-unity-part-1

    // Move Enemies 
    //   [HideInInspector]
    public List<WorldTile> waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    public float speed = 1.0f;


}
