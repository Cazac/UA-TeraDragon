using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public int MaxLives;
    public int CurrentLives;

    [Header("UI ELements")]
    Text crystalText_Red;
    Text crystalText_Blue;
    Text crystalText_Green;
    Text crystalText_Yellow;


    private int crystalsOwned_Red;
    private int crystalsOwned_Blue;
    private int crystalsOwned_Green;
    private int crystalsOwned_Yellow;

    private float crystalsPerSecond_Red;
    private float crystalsPerSecond_Blue;
    private float crystalsPerSecond_Green;
    private float crystalsPerSecond_Yellow;

    /////////////////////////////////////////////////////////////////

    private void Start()
    {
        CurrentLives = MaxLives;

        //Start the crystal chekcer
        StartCoroutine(UpdateCrystalValues());
    }

    public void RemoveLife(int i)
    {
        CurrentLives -= i;
        Debug.Log("Lose " + i + " lives, Current Lives:" + CurrentLives);
        if (CurrentLives < 0)
        {
            CurrentLives = 0;
        }

        if (CurrentLives == 0)
        {
            GameOver();
        }

    }

    public int CheckLife()
    {
        return CurrentLives;
    }

    public void GameOver()
    {
        GameOverScript gos = (GameOverScript) FindObjectOfType(typeof(GameOverScript));
        gos.TurnOnGameOver();
        Debug.Log("Game over man, Game over");
    }

    public IEnumerator UpdateCrystalValues()
    {



        UpdateCrystalUI();

        yield return new WaitForSeconds(1);


        GameOverScript gos = (GameOverScript)FindObjectOfType(typeof(GameOverScript));

        if (!gos.isGameOver)
        {
            //Restart the crystal methods
            StartCoroutine(UpdateCrystalValues());
        }
    }

    public void UpdateCrystalUI()
    {
        crystalText_Red.text = "Red Gems: " + crystalsOwned_Red;
        crystalText_Blue.text = "Red Gems: " + crystalsOwned_Blue;
        crystalText_Green.text = "Red Gems: " + crystalsOwned_Green;
        crystalText_Yellow.text = "Red Gems: " + crystalsOwned_Yellow;
    }

}
