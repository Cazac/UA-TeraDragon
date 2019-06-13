using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundPanAround : MonoBehaviour
{
    public float mouseX;
    public float mouseY;
    public float smoothSpeed;
    public float moveSensitivity;

    public Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseX += Input.GetAxis("Mouse X") * moveSensitivity;
        mouseY += Input.GetAxis("Mouse Y") * moveSensitivity;

        mouseX = Mathf.Clamp(mouseX, -20, 20);
        mouseY = Mathf.Clamp(mouseY, -5, 5);

        targetPosition = new Vector3(mouseX, mouseY, -10);
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * smoothSpeed);

        //this.transform.Translate(mouseX, mouseY, 0);
    }
}
