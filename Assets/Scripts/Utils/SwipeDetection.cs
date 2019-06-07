using UnityEngine;
using System;
using System.Collections;

public class SwipeDetection
{

    private bool isSwiping;
    public bool IsSwiping{ get => isSwiping;}
    private float deltaSwipe;
    public float DeltaSwipe { get => deltaSwipe;}

    private bool isKeyboardInput;
    public bool IsKeyboardInput { get => isKeyboardInput; set => isKeyboardInput = value; }


    private Vector3 firstMousePos;
    public void DetectingSwipe()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            firstMousePos = Input.mousePosition;
            isSwiping = false;
        }

        if(Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            // deltaSwipe = new Vector2(Input.mousePosition.x - firstMousePos.x, 0).magnitude;
            deltaSwipe = Input.mousePosition.x - firstMousePos.x;
            isSwiping = true;
        }

        if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            deltaSwipe = 0;
            isSwiping = false;
        }
    }

    public void DetectingKeyboardInput()
    {
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            IsKeyboardInput = true;
        }

        if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            IsKeyboardInput = false;
        }
    }
}