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
        Move2();
    }

    private void FixedUpdate() {
        
    }

    void Move1() {
        Vector3 startPosition = waypoints[currentWaypoint].transform.position;
        Vector3 endPosition = waypoints[currentWaypoint + 1].transform.position;
        // 2 
        float pathLength = Vector3.Distance(startPosition, endPosition);
        float totalTimeForPath = pathLength / speed;
        float currentTimeOnPath = Time.time - lastWaypointSwitchTime;
        gameObject.transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
        // 3 
        if (gameObject.transform.position.Equals(endPosition)) {
            if (currentWaypoint < waypoints.Length - 2) {
                // 3.a 
                currentWaypoint++;
                lastWaypointSwitchTime = Time.time;
                // TODO: Rotate into move direction
            }
            else {
                // 3.b 
                Destroy(gameObject);

            }
        }
    }

    void Move2() {
        Vector3 startPosition = waypoints[currentWaypoint].transform.position;
        Vector3 endPosition = waypoints[currentWaypoint + 1].transform.position;

        Vector3 dir = waypoints[currentWaypoint + 1].transform.position - waypoints[currentWaypoint].transform.position;

        transform.position += dir.normalized * speed * Time.deltaTime;

        if (Vector3.Distance(gameObject.transform.position,endPosition) < 1f) {
            if (currentWaypoint < waypoints.Length - 2) {
                // 3.a 
                currentWaypoint++;
                lastWaypointSwitchTime = Time.time;
                // TODO: Rotate into move direction
            }
            else {
                // 3.b 
                Destroy(gameObject);

            }
        }

    }

    // Copying tutorial: https://www.raywenderlich.com/269-how-to-create-a-tower-defense-game-in-unity-part-1

    // Move Enemies 
    //   [HideInInspector]
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    public float speed = 1.0f;


}
