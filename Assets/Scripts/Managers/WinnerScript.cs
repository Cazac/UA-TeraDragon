using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerScript : MonoBehaviour
{
    public bool isWinner { get; set; }
    public GameObject WinnerPanel;

    public void TurnOnWinner()
    {
        isWinner = true;
        WinnerPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void TurnOffWinner()
    {
        WinnerPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
