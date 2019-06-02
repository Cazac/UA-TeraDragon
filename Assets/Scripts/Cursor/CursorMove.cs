using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

///////////////
/// <summary>
///     
/// CursorMove
/// 
/// </summary>
///////////////

public class CursorMove : MonoBehaviour
{
    //Public Variables
    public float speed;
    public Tilemap tilemap;
    public Vector3 BottomLeftLimit;
    public Vector3 TopRightLimit;

    //Private Variables
    private Rigidbody2D cursorRB;
    private BoxCollider2D collider2d;
    private Vector3 change;
    private float gridSize;

    /////////////////////////////////////////////////////////////////

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        MoveCursor();
    }

    ///////////////
    /// <summary>
    /// Undocumented
    /// </summary>
    ///////////////
    private void Setup()
    {
        gridSize = tilemap.cellSize.x;
        cursorRB = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<BoxCollider2D>();
    }


    ///////////////
    /// <summary>
    /// Undocumented
    /// </summary>
    ///////////////
    private void MoveCursor()
    {
        change = Vector3.zero;
        change.x = Input.mousePosition.x;
        change.y = Input.mousePosition.y;

        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(change);
        Vector3 roundedPos = new Vector3(gridSize/2+ Mathf.Floor(cursorPos.x / gridSize) * gridSize,
                                         gridSize / 2 + Mathf.Floor(cursorPos.y / gridSize) * gridSize);

        cursorRB.MovePosition(Vector3.Lerp(transform.position, roundedPos, speed));
    }
}
