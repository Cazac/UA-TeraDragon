using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuScript : MonoBehaviour
{
    public bool isGameOver { get; set; }

    public GameObject PlayMenu;
    public GameObject HelpMenu;



    void Start()
    {
        //PlayMenu.SetActive(true);
        HelpMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void TurnOnHelp()
    {
        Time.timeScale = 0;
        HelpMenu.SetActive(true);
    }

    public void TurnOffHelp()
    {
        Time.timeScale = 1;
        HelpMenu.SetActive(false);
    }
}
