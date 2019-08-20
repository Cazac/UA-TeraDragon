using UnityEngine;
using System;
using System.Collections;
public class InputDetection
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

    
    public String DetectingKeyboardInputEvent()
    {
        if(Input.GetKey(KeyCode.A))
        {
            IsKeyboardInput = true;
            return "Left";
        }

        if(Input.GetKey(KeyCode.D))
        {
            IsKeyboardInput = true;
            return "Right";
        }

        if(Input.GetKey(KeyCode.W))
        {
            IsKeyboardInput = true;
            return "Up";
        }

            
        if(Input.GetKey(KeyCode.S))
        {
            IsKeyboardInput = true;
            return "Down";
        }

        if(!Input.anyKey)
        {
            IsKeyboardInput = false;
            return "None";
        }

        return "None";
    }

      /// <summary>
    ///Detect if left or right mouse button is pressed
    /// </summary>
    ///<returns>String stating whhich event has been captured(click, press, scroll,...)</returns> 
    public String BeginClickEvent()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            return "Clicked";

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            return "Pressed";

        if (Input.mouseScrollDelta != new Vector2(0, 0))
            return "Scrolling";


        return "None";
    }

    public String MouseReleaseEvent()
    {
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            return "Released";

        return "None";
    }
}