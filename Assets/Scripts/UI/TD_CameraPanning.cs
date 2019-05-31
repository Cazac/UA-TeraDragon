using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////
/// <summary>
///     
/// UnitMono is the Mono Version of a Unit, this is used as the actual gameobject and is deconstucted on saving.
/// 
/// </summary>
///////////////

public class TD_CameraPanning : MonoBehaviour
{
    ////////////////////////////////

    public GameObject movingCamera;

    public GameObject[] CameraNodes;

    public GameObject currentNode;
    public GameObject targetNode;

    ////////////////////////////////

    public int currentNodeCounter = 0;

    public float IncrementPerNode = 3f;

    public float currentIncrement = 0;
    public float currentProgress = 0;

    /////////////////////////////////////////////////////////////////

    private void Start()
    {
        currentNode = CameraNodes[currentNodeCounter];
        targetNode = CameraNodes[currentNodeCounter + 1];

        print("Setting Current Node " + currentNodeCounter);
        print("Setting Target Node " + (currentNodeCounter + 1));
    }

    private void Update()
    {
        //
        if (currentProgress >= 1)
        {
            //
            if ((currentNodeCounter + 2) >= (CameraNodes.Length))
            {



                currentNode = CameraNodes[CameraNodes.Length - 1];
                targetNode = CameraNodes[0];

                print("New Current Node " + (CameraNodes.Length - 1));
                print("New Target Node " + 0);

                //Over cap reset
                currentNodeCounter = -1;
                print("Reset Node Counter");
            }
            else
            {
                //Add one
                currentNodeCounter++;

                print("Add To Node Counter");

                currentNode = CameraNodes[currentNodeCounter];
                targetNode = CameraNodes[currentNodeCounter + 1];

                print("New Current Node " + currentNodeCounter);
                print("New Target Node " + (currentNodeCounter + 1));
            }

        

            //Reset the increment counter
            currentIncrement = 0;
        }



        currentProgress = currentIncrement / IncrementPerNode;


        Vector3 start_V3 = currentNode.transform.position;
        Vector3 end_V3 = targetNode.transform.position;

        //Set New Position Based on Progress
        movingCamera.transform.position = Vector3.Lerp(start_V3, end_V3, currentProgress);

        //Update the progress ??????
        currentIncrement += Time.deltaTime;
    }
   
    /////////////////////////////////////////////////////////////////
}

