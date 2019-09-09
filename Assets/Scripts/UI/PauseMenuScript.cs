using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public bool isGameOver { get; set; }

    public GameObject PlayMenu;
    public GameObject PauseMenu;
    public GameObject UpgradeMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (isGameOver)
            {
                TurnOffUpgrade();
            }
            else
            {
                TurnOnUpgrade();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayMenu.SetActive(true);
        PauseMenu.SetActive(false);
        UpgradeMenu.SetActive(false);
        isGameOver = false;
    }
    
    public void TurnOnPause()
    {
        Deselecting();
        isGameOver = true;
        Time.timeScale = 0;
        PlayMenu.SetActive(false);
        UpgradeMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }

    public void TurnOffPause()
    {
        isGameOver = false;
        Time.timeScale = 1;
        PlayMenu.SetActive(true);
        UpgradeMenu.SetActive(false);
        PauseMenu.SetActive(false);
    }

    public void TurnOnUpgrade()
    {
        Deselecting();
        isGameOver = true;
        Time.timeScale = 0;
        PlayMenu.SetActive(false);
        UpgradeMenu.SetActive(true);
        PauseMenu.SetActive(false);
    }

    public void TurnOffUpgrade()
    {
        Deselecting();
        isGameOver = false;
        Time.timeScale = 1;
        PlayMenu.SetActive(true);
        UpgradeMenu.SetActive(false);
        PauseMenu.SetActive(false);
    }

    //  Functions for buttons in pause panels

    public void GoToMainMenuButton()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void UnpauseButton()
    {
        TurnOffPause();
    }

    public void Deselecting()
    {
        TowerDrag[] towerDrags = FindObjectsOfType<TowerDrag>();
        foreach(TowerDrag td in towerDrags)
        {
            td.ManualDeselect();
        }
        MinerDrag[] minerDrag = FindObjectsOfType<MinerDrag>();
        foreach (MinerDrag md in minerDrag)
        {
            md.ManualDeselect();
        }
    }

}
