using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Tilemaps;

public class CameraPanningCursor : MonoBehaviour
{

    [Header("Screen lerping speed")]
    public float lerpSpeed;

    [Header("Layer for play area")]

    // public int playAreaBorder;

    [Header("Zoom threshold")]
    public int zoomMin;
    public int zoomMax;

    private Camera camera;
    public Vector2 CameraBoundingX { get; set; }
    public Vector2 CameraBoundingY { get; set; }

    private SwipeDetection swipeDetection = new SwipeDetection();


    public GameObject playArea;

    private void Start()
    {
        // playAreaBorder = 1 << playAreaBorder;
        camera = Camera.main;


        if(playArea.GetComponent<Tilemap>() == null)
        {
            Debug.LogError("TileMap not found");
        }

        else
        {
            CameraBoundingX = new Vector2(-GetTileMapSize(playArea.GetComponent<Tilemap>()).x / 2, 
            GetTileMapSize(playArea.GetComponent<Tilemap>()).x / 2);

            CameraBoundingY = new Vector2(-GetTileMapSize(playArea.GetComponent<Tilemap>()).y / 2, 
            GetTileMapSize(playArea.GetComponent<Tilemap>()).y / 2);
        }

        Debug.Log(CameraBoundingX.ToString());

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

        swipeDetection.DetectingSwipe();
    }

    private void LateUpdate() 
    {
        if(!IsInsideBound())
        {
            ReshiftCam(CameraBoundingX, CameraBoundingY);
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


            if(IsInsideBound() && swipeDetection.IsSwiping)  
            {
                camera.transform.position = new Vector3(camera.transform.position.x + swipeDetection.DeltaSwipe * -lerpSpeed * Time.deltaTime, camera.transform.position.y,
                camera.transform.position.z );
            }
        }
    }

    public bool IsInsideBound()
    {
        if(((camera.transform.position.x > CameraBoundingX.x) && (camera.transform.position.x < CameraBoundingX.y)))  
            return true;
        return false;
    }


    private void ReshiftCam(Vector2 boundingX, Vector2 boundingY)
    {

        if(camera.transform.position.x > boundingX.y)
        {
            Debug.Log("Reshift right bound");
            camera.transform.position = new Vector3(boundingX.y - 1, camera.transform.position.y, camera.transform.position.z);
        }
        
        if( camera.transform.position.x < boundingX.x)
        {
            Debug.Log("Reshift left bound");
            camera.transform.position = new Vector3(boundingX.x + 1, camera.transform.position.y, camera.transform.position.z);
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
    ///<returns>False if raycast hits tilemap box collider, otherwise true</returns> 
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
            hitLayer = 1 << hitLayer;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity, hitLayer);
            if(hit.collider != null)
            {
                Bounds bounds = playArea.GetComponent<Tilemap>().localBounds;
                return false;
            }
        }

        return true;
    }

    private Vector3 GetTileMapSize(Tilemap playArea)
    {
        Bounds bounds = playArea.GetComponent<Tilemap>().localBounds;
        return bounds.size;
    }
}


