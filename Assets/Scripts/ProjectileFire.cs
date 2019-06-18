using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFire : MonoBehaviour
{

    public GameObject enemy;


    public float speed = 1000;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check if Valid
        //CheckTarget();

        //Move position closer for collision
        ChaseTarget();
    }




    public void ChaseTarget()
    {
        if (enemy == null)
        {
            print("NOOO");
            Destroy(gameObject);
        }
        else
        {
            Vector3 target = enemy.transform.position;
            float step = speed * Time.deltaTime;

            //gameObject.transform.LookAt(target);


            //Look at?
            transform.right = target - transform.position;


            transform.position = Vector2.MoveTowards(gameObject.transform.position, target, 1);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        //Deal damage
        //Destroy Project

        if (collider.gameObject == enemy)
        {
            print("HERE");
        }
        else
        {
            print("WRONG");
        }
  


    }


    public void CheckTarget()
    {
        
    }

}
