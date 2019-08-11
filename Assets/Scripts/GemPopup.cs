using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemPopup : MonoBehaviour
{
    //The gameobject Text 
    private TextMeshPro textMesh;

    //How long the text lasts
    private const float DISAPPEAR_TIMER_MAX = 1f;

    //Static Varible to Determin height of text
    private static int sortingOrder;

    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;


    /////////////////////////////////////////////////////////////////

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        MoveText();
    }

    /////////////////////////////////////////////////////////////////

    public void Setup(int gemsAdded, Color color)
    {
        //Set value
        textMesh.SetText("+" + gemsAdded.ToString());

        //Set Timer for text to disapear
        disappearTimer = DISAPPEAR_TIMER_MAX;

        //Increment / set Layer for better visablity
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        //Set Color
        textMesh.color = color;

        // ???
        float randomX = Random.Range(-2f, 2f);
        moveVector = new Vector3(randomX, 1) * 60f;
    }


    public void MoveText()
    {
        //Get Values
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 30f * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            // First half of the popup lifetime
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }


        // ???
        disappearTimer -= Time.deltaTime;

        // Start disappearing
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
