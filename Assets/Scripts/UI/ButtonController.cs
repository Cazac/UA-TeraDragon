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

    ///////////////////////////////////////////////////////////////// - Game Audio Buttons

    public void ButtonGame_MuteMusic()
    {
        soundManager.MuteSoundtrack();

        if (soundManager.IsMuteSoundtrack)
        {
            gameMuteMusic_TEXT.text = "Unmute SoundTrack";
        }
        else
        {
            gameMuteMusic_TEXT.text = "Mute SoundTrack";
        }
    }

    public void ButtonGame_MuteSFX()
    {
        soundManager.MuteUI();

        if (soundManager.IsMuteUI)
        {
            gameMuteSFX_TEXT.text = "Unmute UI";
        }
        else
        {
            gameMuteSFX_TEXT.text = "Mute UI";
        }
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
        soundManager.MuteSoundtrack();

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
        soundManager.MuteUI();

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
