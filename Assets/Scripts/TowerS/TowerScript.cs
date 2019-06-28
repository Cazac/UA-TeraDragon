using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float timer = 5f;
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            ShootTarget();
            timer = 5f;
        }
    }

    void ShootTarget()
    {
        print(closestPosition);
    }

    Vector3 closestPosition;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.transform.position);
        if(Vector3.Distance(transform.position, collision.transform.position) < Vector3.Distance(transform.position, closestPosition))
        {
            closestPosition = collision.transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.transform.position);
        if (Vector3.Distance(transform.position, collision.transform.position) < Vector3.Distance(transform.position, closestPosition))
        {
            closestPosition = collision.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.transform.position);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("Collider stay:" + collision.transform.position);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger 3d stay:" + other.transform.position);
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collision 3d stay:" + collision.transform.position);
    }

}
