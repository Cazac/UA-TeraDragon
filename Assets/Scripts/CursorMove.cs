using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMove : MonoBehaviour
{

    //Public Variables
    public float speed;
    public float gridSize;
    public Vector3 BottomLeftLimit;
    public Vector3 TopRightLimit;

    //Private Variables
    private Rigidbody2D cursorRB;
    private BoxCollider2D collider2d;
    private Vector3 change;

    // Start is called before the first frame update
    void Start()
    {
        cursorRB = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        change.x = Input.mousePosition.x;
        change.y = Input.mousePosition.y;
        MoveCursor();
    }

    void MoveCursor()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(change);
        Vector3 roundedPos = new Vector3(Mathf.Round(cursorPos.x / gridSize) * gridSize, 
                                         Mathf.Round(cursorPos.y / gridSize) * gridSize);

        cursorRB.MovePosition(Vector3.Lerp(transform.position, roundedPos, speed));
    }
}
