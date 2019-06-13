using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public GameObject GameOverPanel;

    public void TurnOnGameOver()
    {
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
