using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Tilemaps;

public class CameraPanningCursor : MonoBehaviour
{

    [Header("Screen lerping speed")]
    public float lerpSpeed;

    [Header("Layer for play area")]
    public int playAreaBorder;

    [Header("Zoom threshold")]
    public int zoomMin;
    public int zoomMax;

    private Camera camera;

    public GameObject playArea;

    private void Start()
    {
        playAreaBorder = 1 << playAreaBorder;
        camera = Camera.main;
    }

    private void Update()
    {
        if (InputClickEvent() == "Pressed")
        {
            TranslateWithMousePos();
        }

        if (InputClickEvent() == "Scrolling")
        {
            ZoomWithMouseWheel();
        }
    }

    /// <summary>
    ///Main function to perform camera panning, function is called when camera is not out of play area bound, uses Vector3.Lerp()
    /// </summary>
    /// <example>
    /// <code>
    ///Vector3.Lerp(camera.transform.position, mousePosToWorld, lerpSpeed*Time.deltaTime);
    /// </code>
    /// </example>
    private void TranslateWithMousePos()
    {
        if (camera != null && !CameraLock())
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 mousePosToWorld = Camera.main.ScreenToWorldPoint(mousePosition);
            camera.transform.position = Vector3.Lerp(camera.transform.position, mousePosToWorld, lerpSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    ///Main function to perform camera zooming, function is called when camera size is inside a predefined range, uses Mathf.Lerp()
    /// </summary>
    /// <example>
    /// <code>
    /// Mathf.Lerp(camera.orthographicSize, zoomThreshold, time);
    /// </code>
    /// </example>
    private void ZoomWithMouseWheel()
    {
        if (camera.orthographicSize < zoomMin)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoomMin, 0.8f);
        }

        if (camera.orthographicSize > zoomMax)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoomMax, 0.8f); ;
        }
        camera.orthographicSize += -Input.mouseScrollDelta.y;

    }

    /// <summary>
    ///Detect if left or right mouse button is pressed
    /// </summary>
    ///<returns>String stating whhich event has been captured(click, press, scroll,...)</returns> 
    public String InputClickEvent()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            return "Pressed";
        }

        if (Input.mouseScrollDelta != new Vector2(0, 0))
        {
            return "Scrolling";
        }


        return "None";
    }


    /// <summary>
    ///Detect if camera is out of play area by using Raycast
    /// </summary>
    /// <param name="playAreaBorder">The layer of the play area</param>
    ///<returns>True if raycast is hit, otherwise false</returns> 
    private bool CameraLock(int playAreaBorder)
    {
        Ray raycastMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(raycastMouse, out hit, Mathf.Infinity, playAreaBorder))
        {
            return false;
        }

        else
        {
            return true;
        }
    }


    /// <summary>
    ///Detect if camera is out of play area by using Raycast
    /// </summary>
    /// <param name="playAreaBorder">The layer of the play area</param>
    ///<returns>True if raycast is hit, otherwise false</returns> 
    private bool CameraLock()
    {
        Ray raycastMouse = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (playArea.gameObject == null)
        {
            Debug.LogError("Play area is not assigned in CameraPanning.cs");
        }

        else
        {
            //Performs bit shifting:
            int hitLayer = playArea.gameObject.layer;
            Debug.Log(hitLayer);
            hitLayer = 1 << hitLayer;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity, hitLayer);
            Debug.DrawRay(Input.mousePosition, Vector3.forward, Color.red);
            Debug.Log(hit.collider);
            if(hit.collider != null)
            {
                Bounds bounds = playArea.GetComponent<Tilemap>().localBounds;
                Debug.Log(bounds.size.ToString());
                return false;
            }
        }

        return true;
    }

    public static class CameraUtils
    {
        public static Vector3 TilemapSize { get; set; }
    }

}


