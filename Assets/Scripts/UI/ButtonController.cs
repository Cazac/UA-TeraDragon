using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

///////////////
/// <summary>
///     
/// TD_ButtonController is used to manage all of the button inputs
/// 
/// </summary>
///////////////

public class ButtonController : MonoBehaviour
{



    ////////////////////////////////
    
    [Header("Menu Reffs")]
    public Text menuMuteMusic_TEXT;
    public Text menuMuteSFX_TEXT;

    public GameObject menuSettingPanel;


    [Header("Game Reffs")]



    private bool isMusicPlaying;
    private bool isSFXPlaying;

    SoundManager soundManager;

    /////////////////////////////////////////////////////////////////

    public void Start()
    {
        soundManager = GameObject.FindObjectOfType<SoundManager>();
    }

    ///////////////////////////////////////////////////////////////// - Extra Buttons

    public void ButtonGame_StartNextWave()
    {
        Debug.Log("Wave");
    }

    public void ButtonGame_RestartGame()
    {
        //Load into main game
        SceneManager.LoadScene("Main Game (Full Merge)");
    }

    public void ButtonGame_ResumeGame()
    {
        //Unpause game
        PauseMenuScript pauseScript = GameObject.FindObjectOfType<PauseMenuScript>();
        pauseScript.TurnOffPause();
    }

    public void ButtonGame_QuitGame()
    {
        //Close game
        Application.Quit();
    }

    public void ButtonGame_ReturnToMenu()
    {
        //Load into main game
        SceneManager.LoadScene("Main Menu");
    }

    ///////////////////////////////////////////////////////////////// - Main Menu

    public void ButtonMenu_Play()
    {
        //Load into main game
        SceneManager.LoadScene("Main Game (Full Merge)");
    }

    public void ButtonMenu_Settings()
    {
        //Open Settings
        menuSettingPanel.SetActive(!menuSettingPanel.activeSelf);
    }

    public void ButtonMenu_Credits()
    {
        //Open Settings
        menuSettingPanel.SetActive(!menuSettingPanel.activeSelf);
    }

    public void ButtonMenu_Quit()
    {
        //Close game
        Application.Quit();
    }

    ///////////////////////////////////////////////////////////////// - Audio Menu

    public void ButtonMenu_MuteMusic()
    {
        Debug.Log("Mute Music");

        if (soundManager.IsMuteSoundtrack)
        {
            menuMuteMusic_TEXT.text = "Unmute SoundTrack";
        }
        else
        {
            menuMuteMusic_TEXT.text = "Mute SoundTrack";
        }
    }

    public void ButtonMenu_MuteSFX()
    {
        Debug.Log("Mute SFX");

        if (soundManager.IsMuteUI)
        {
            menuMuteSFX_TEXT.text = "Unmute UI";
        }
        else
        {
            menuMuteSFX_TEXT.text = "Mute UI";
        }
    }

    /////////////////////////////////////////////////////////////////
}
