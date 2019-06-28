using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public bool isGameOver { get; set; }
    public GameObject PauseMenu;
    public GameObject PlayMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isGameOver)
            {
                TurnOffPause();
            }
            else
            {
                TurnOnPause();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayMenu.SetActive(true);
        PauseMenu.SetActive(false);
        isGameOver = false;
    }
    
    void TurnOnPause()
    {
        isGameOver = true;
        Time.timeScale = 0;
        PlayMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }

    void TurnOffPause()
    {
        isGameOver = false;
        Time.timeScale = 1;
        PlayMenu.SetActive(true);
        PauseMenu.SetActive(false);
    }

    //  Functions for buttons in pause panels

    public void GoToMainMenuButton()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void UnpuaseButton()
    {
        TurnOffPause();
    }

}
