using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Tilemaps;

public class CameraPanningCursor : MonoBehaviour
{

    [Header("Camera lerping speed with mouse")]
    [Range(0, 2)]
    public float cameraLerpSpeed = 1;

    [Header("Zoom threshold")]
    public int zoomMin;
    public int zoomMax;

    [Header("Camera lerping speed with keyboard")]
    [Range(0, 5)]
    public float keyboardLerpSpeed = 2;

    [Header("Camera zoom speed")]
    [Range(0, 2)]
    public float zoomSpeed = 1;

    [Header("Camera bounce speed ")]
    [Range(0, 1)]
    public float bounceSpeed = 0.045f;

    private Camera camera;
    public Vector2 CameraBoundingX { get; set; }
    public Vector2 CameraBoundingY { get; set; }

    private InputDetection inputDetection = new InputDetection();


    public GameObject playArea;

    private void Start()
    {
        // playAreaBorder = 1 << playAreaBorder;
        camera = Camera.main;


        if (playArea.GetComponent<Tilemap>() == null)
        {
            Debug.LogError("TileMap not found");
        }

        else
        {
            CameraBoundingX = new Vector2(-GetTileMapSize(playArea.GetComponent<Tilemap>()).x / 2,
            GetTileMapSize(playArea.GetComponent<Tilemap>()).x / 2);

            CameraBoundingY = new Vector2(GetTileMapSize(playArea.GetComponent<Tilemap>()).y / 2,
            -GetTileMapSize(playArea.GetComponent<Tilemap>()).y / 2);
        }

        StartCoroutine(ReshiftCam(CameraBoundingX, CameraBoundingY));

        SetCameraBounds(5);
    }

    /// <summary>
    ///Coroutine to reshift camera position when camera is out of predefined bound
    /// </summary>
    /// <param name="boundingX"> Play area left and right width bound</param>
    /// <param name="boundingY"> Play area upmost and downmost bound</param>
    ///<returns>Returns null each frame as base condition for coroutine</returns> 
    private IEnumerator ReshiftCam(Vector2 boundingX, Vector2 boundingY)
    {
        while (true)
        {
            if (camera.transform.position.x > boundingX.y)
            {
                camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(CameraBoundingX.y - 1, camera.transform.position.y, camera.transform.position.z), bounceSpeed);
            }

            if (camera.transform.position.x < boundingX.x)
            {
                camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(CameraBoundingX.x + 1, camera.transform.position.y, camera.transform.position.z), bounceSpeed);
            }

            if (camera.orthographicSize < zoomMin)
            {
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoomMin, bounceSpeed);
            }

            if (camera.orthographicSize > zoomMax)
            {
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoomMax, bounceSpeed);
            }
            yield return null;
        }
    }

    private void Update()
    {

        inputDetection.DetectingSwipe();
        inputDetection.DetectingKeyboardInputEvent();

        if (inputDetection.BeginClickEvent() == "Pressed")
        {
            TranslateWithMousePos();
        }

        if (inputDetection.BeginClickEvent() == "Scrolling")
        {
            ZoomWithMouseWheel();
        }

        if (inputDetection.IsKeyboardInput)
        {
            TranslateWithKeyboard();
        }
    }

    private void LateUpdate()
    {
        if (!IsInsideBound())
        {
            ReshiftCam(CameraBoundingX, CameraBoundingY);
        }
    }

    /// <summary>
    ///     Main function to perform camera panning, function is called when camera is not out of play area bound, uses Vector3.Lerp()
    /// </summary>
    /// <example>
    ///     Vector3.Lerp(camera.transform.position, mousePosToWorld, cameraLerpSpeed*Time.deltaTime);
    /// </example>
    private void TranslateWithMousePos()
    {
        if (camera != null && !CameraLock())
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 mousePosToWorld = Camera.main.ScreenToWorldPoint(mousePosition);


            if (IsInsideBound() && inputDetection.IsSwiping)
            {
                camera.transform.position = new Vector3(camera.transform.position.x + inputDetection.DeltaSwipe * -cameraLerpSpeed * Time.deltaTime, camera.transform.position.y,
                camera.transform.position.z);
            }
        }
    }



     /// <summary>
     ///Check if camera is inside play area bound
    /// </summary>
    /// <param name="CameraBoundingX"> Play area left and right width bound</param>
    /// <param name="CameraBoundingY"> Play area upmost and downmost bound</param>
    /// <para></para> 
    /// <example>
    /// <code>
    /// </code>
    /// </example>
    ///<returns>True if camera position is inside play area.Otherwuse false</returns> 
    public bool IsInsideBound()
    {
        if (((camera.transform.position.x > CameraBoundingX.x) && (camera.transform.position.x < CameraBoundingX.y)))
            return true;
        return false;
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
        camera.orthographicSize += -Input.mouseScrollDelta.y * zoomSpeed;

    }

    private void TranslateWithKeyboard()
    {
        if (inputDetection.DetectingKeyboardInputEvent() == "Left")
        {
            camera.transform.position = new Vector3(camera.transform.position.x - keyboardLerpSpeed, camera.transform.position.y,
            camera.transform.position.z);
        }

        if (inputDetection.DetectingKeyboardInputEvent() == "Right")
        {
            camera.transform.position = new Vector3(camera.transform.position.x + keyboardLerpSpeed, camera.transform.position.y,
            camera.transform.position.z);
        }
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
            if (hit.collider != null)
            {
                Bounds bounds = playArea.GetComponent<Tilemap>().localBounds;
                return false;
            }
        }

        return true;
    }


     /// <summary>
     ///Get the size of tilemap using Bounds class
    /// </summary>
    /// <param name="playArea">Play area that contains Tilemap component</param>
    ///<returns>Size of tilemap as vector3</returns> 
    private Vector3 GetTileMapSize(Tilemap playArea)
    {
        Bounds bounds = playArea.GetComponent<Tilemap>().localBounds;
        return bounds.size;
    }


    ///////////////
    /// <summary>
    /// TO DO, Does not sync well with the given bounds, how to convert?
    /// </summary>
    ///////////////
    public void SetCameraBounds(float boundRight)
    {
        //Vector3 tileSize = GetTileMapSize(playArea.GetComponent<Tilemap>());


        //Multiple the base value by tile size
        //boundRight = boundRight * 11.2f;
        //print(boundRight);

        //CameraBoundingX = new Vector2(-boundRight, boundRight);

        //StartCoroutine(ReshiftCam(CameraBoundingX, CameraBoundingY));
    }
}


