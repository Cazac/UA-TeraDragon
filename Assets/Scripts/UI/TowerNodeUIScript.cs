using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerNodeUIScript : MonoBehaviour
{
    public Text NodeText;

    private void Start()
    {
        NodeText = GetComponentInChildren<Text>();
     //   NodeText.text = "test start";
    }

    private void Update()
    {
        if(NodeText == null)
        {
            Debug.Log("Test is null");
        }


    }

    public void changeNodeText(string textInput)
    {
     //   NodeText = GetComponentInChildren<Text>();
        Debug.Log("SET TEXT CALLED");
        NodeText.text = textInput;
    }
}
