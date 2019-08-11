using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{

    public float TimeTillDestruction;
    public float counter;


    public void Setup(float DestroyTime)
    {
        TimeTillDestruction = DestroyTime + 0.2f;
        counter = 0;
    }

    public void Update()
    {
        //if (TimeTillDestruction == 0)
        ////
            //return;
        //}


        if (TimeTillDestruction < counter)
        {
            Destroy(gameObject);
        }


        counter += Time.deltaTime;
    }
}
