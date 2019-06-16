using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public bool isGameOver { get; set; }
    public GameObject GameOverPanel;

    public void TurnOnGameOver()
    {
        isGameOver = true;
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void TurnOffGameOver()
    {
        GameOverPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void TestingButton()
    {
        SceneManager.LoadScene(1);
    }


}
