using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////
/// <summary>
///     
/// TD_CameraPanning is used to move the camera around the title screen
/// 
/// </summary>
///////////////

public class TD_CameraPanning : MonoBehaviour
{
    [Header("Camera")]
    public GameObject movingCamera;

    [Header("Position Nodes")]
    public GameObject[] CameraNodes;

    private GameObject currentNode;
    private GameObject targetNode;

    [Header("Speed")]
    public float IncrementPerNode = 3f;

    private int currentNodeCounter = 0;
    private float currentIncrement = 0;
    private float currentProgress = 0;

    /////////////////////////////////////////////////////////////////

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        MoveCamera();
    }


    ///////////////
    /// <summary>
    /// Setup variables for first pass of camera movement
    /// </summary>
    ///////////////
    private void Setup()
    {
        currentNode = CameraNodes[currentNodeCounter];
        targetNode = CameraNodes[currentNodeCounter + 1];
    }


    ///////////////
    /// <summary>
    /// Loop through each node placement and move the camera with lerp
    /// </summary>
    ///////////////
    private void MoveCamera()
    {

        if (currentProgress >= 1)
        {
            if ((currentNodeCounter + 2) >= (CameraNodes.Length))
            {
                //Go back to first node
                currentNode = CameraNodes[CameraNodes.Length - 1];
                targetNode = CameraNodes[0];

                //Over the cap reset for next pass
                currentNodeCounter = -1;
            }
            else
            {
                //Continue to next node
                currentNodeCounter++;

                currentNode = CameraNodes[currentNodeCounter];
                targetNode = CameraNodes[currentNodeCounter + 1];
            }

            currentIncrement = 0;
        }

        Vector3 start_V3 = currentNode.transform.position;
        Vector3 end_V3 = targetNode.transform.position;

        //Set New Position Based on Progress
        currentProgress = currentIncrement / IncrementPerNode;
        movingCamera.transform.position = Vector3.Lerp(start_V3, end_V3, currentProgress);

        //Update the progress
        currentIncrement += Time.deltaTime;
    }

}

