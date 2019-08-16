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
    
    [Header("Menu Music Text")]
    public Text menuMuteMusic_TEXT;
    public Text menuMuteSFX_TEXT;

    [Header("Game Music Text")]
    public Text gameMuteMusic_TEXT;
    public Text gameMuteSFX_TEXT;

    [Header("Game Reffs")]
    public GameObject menuSettingPanel;



    private bool isMusicPlaying;
    private bool isSFXPlaying;

    private SoundManager soundManager;

    /////////////////////////////////////////////////////////////////

    public void Awake()
    {
        soundManager = GameObject.FindObjectOfType<SoundManager>();
    }

    ///////////////////////////////////////////////////////////////// - Game Buttons

    public void ButtonGame_RemoveHelp()
    {
        HelpMenuScript helpScript = GameObject.FindObjectOfType<HelpMenuScript>();
        helpScript.TurnOffHelp();
    }

    public void ButtonGame_Help()
    {
        HelpMenuScript helpScript = GameObject.FindObjectOfType<HelpMenuScript>();
        helpScript.TurnOnHelp();
    }

    public void ButtonGame_RestartGame()
    {
        //Time Scale
        Time.timeScale = 1;

        //Load into main game
        SceneManager.LoadScene("Main Game");
    }

    public void ButtonGame_ResumeGame()
    {
        //Unpause game
        PauseMenuScript pauseScript = GameObject.FindObjectOfType<PauseMenuScript>();
        pauseScript.TurnOffPause();
    }

    public void ButtonGame_QuitGame()
    {
        //Time Scale
        Time.timeScale = 1;

        //Close game
        Application.Quit();
    }

    public void ButtonMenu_Credits()
    {
        //Time Scale
        Time.timeScale = 1;

        //Load into credits
        SceneManager.LoadScene("Credits");
    }

    public void ButtonGame_ReturnToMenu()
    {
        //Time Scale
        Time.timeScale = 1;

        WinnerScript winner = GameObject.FindObjectOfType<WinnerScript>();
        winner.TurnOffWinner();

        //Load into main game
        SceneManager.LoadScene("Main Menu");

    }

    ///////////////////////////////////////////////////////////////// - Game Audio Buttons

    public void ButtonGame_MuteMusic(Text gameText)
    {
        soundManager.MuteSoundtrack();

        if (soundManager.IsMuteSoundtrack)
        {
            gameText.text = "Music: OFF";
        }
        else
        {
            gameText.text = "Music: ON";
        }
    }

    public void ButtonGame_MuteSFX(Text gameText)
    {
        soundManager.MuteUI();

        if (soundManager.IsMuteUI)
        {
            gameText.text = "SFX: OFF";
        }
        else 
        {
            gameText.text = "SFX: ON";
        }
    }

    ///////////////////////////////////////////////////////////////// - Main Menu

    public void ButtonMenu_Play()
    {
        //Time Scale
        Time.timeScale = 1;

        //Load into main game
        SceneManager.LoadScene("Main Game");
    }

    public void ButtonMenu_Settings()
    {
        //Open Settings
        //menuSettingPanel.SetActive(true);
        menuSettingPanel.SetActive(!menuSettingPanel.activeSelf);
    }

    public void ButtonGame_Credits()
    {
        //Time Scale
        Time.timeScale = 1;

        //Load into credits
        SceneManager.LoadScene("Credits");
    }

    public void ButtonMenu_Quit()
    {
        //Close game
        Application.Quit();
    }

    ///////////////////////////////////////////////////////////////// - Audio Menu

    public void ButtonMenu_MuteMusic()
    {
        soundManager.MuteSoundtrack();

        if (soundManager.IsMuteSoundtrack)
        {
            menuMuteMusic_TEXT.text = "Music: OFF";
        }
        else
        {
            menuMuteMusic_TEXT.text = "Music: ON";
        }
    }

    public void ButtonMenu_MuteSFX()
    {
        soundManager.MuteUI();

        if (soundManager.IsMuteUI)
        {
            menuMuteSFX_TEXT.text = "SFX: OFF";
        }
        else
        {
            menuMuteSFX_TEXT.text = "SFX: ON";
        }
    }

    /////////////////////////////////////////////////////////////////
}
