using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TD_ButtonController : MonoBehaviour
{

    ///////////////
    /// <summary>
    ///     
    /// TD_ButtonController is used to manage all of the button inputs
    /// 
    /// </summary>
    ///////////////

    ////////////////////////////////

    private bool isMusicPlaying;
    private bool isSFXPlaying;

    ///////////////////////////////////////////////////////////////// - Extra uttons

    public void Button_StartNextWave()
    {
        Debug.Log("Wave");
    }

    ///////////////////////////////////////////////////////////////// - Tower Buttons

    public void Button_TowerFire()
    {
        Debug.Log("Tower");
    }

    public void Button_TowerIce()
    {
        Debug.Log("Tower");
    }

    public void Button_TowerEarth()
    {
        Debug.Log("Tower");
    }

    public void Button_TowerLightning()
    {
        Debug.Log("Tower");
    }

    ///////////////////////////////////////////////////////////////// - Skill Buttons

    public void Button_SkillFire()
    {
        Debug.Log("Fire");
    }

    public void Button_SkillIce()
    {
        Debug.Log("Ice");
    }

    public void Button_SkillEarth()
    {
        Debug.Log("Earth");
    }

    public void Button_SkillLightning()
    {
        Debug.Log("Lightning");
    }

    ///////////////////////////////////////////////////////////////// - Main Menu

    public void Button_Play()
    {
        //Load into main game
        SceneManager.LoadScene("Main Game");
    }

    public void Button_Help()
    {
        Debug.Log("Fire");
    }

    public void Button_Credits()
    {
        Debug.Log("Fire");
    }

    public void Button_Quit()
    {
        //Close game
        Application.Quit();
    }

    ///////////////////////////////////////////////////////////////// - Audio Menu

    public void Button_Music()
    {
        Debug.Log("Mute Music");
    }

    public void Button_SFX()
    {
        Debug.Log("Mute SFX");
    }

    /////////////////////////////////////////////////////////////////
}
