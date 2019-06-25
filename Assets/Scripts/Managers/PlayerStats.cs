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
    public Text crystalText_Red;
    public Text crystalText_Blue;
    public Text crystalText_Green;
    public Text crystalText_Yellow;


    public int crystalsOwned_Red;
    public int crystalsOwned_Blue;
    public int crystalsOwned_Green;
    public int crystalsOwned_Yellow;

    private float crystalsExtra_Red;
    private float crystalsExtra_Blue;
    private float crystalsExtra_Green;
    private float crystalsExtra_Yellow;

    private float crystalsPerSecond_Red = 0.5f;
    private float crystalsPerSecond_Blue = 0.1f;
    private float crystalsPerSecond_Green = 0.1f;
    private float crystalsPerSecond_Yellow = 0.1f;

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



    ///////////////
    /// <summary>
    /// Undocumented
    /// </summary>
    ///////////////
    public IEnumerator UpdateCrystalValues()
    {
        //Upadate Extra Values
        crystalsExtra_Red += crystalsPerSecond_Red;
        crystalsExtra_Blue += crystalsPerSecond_Blue;
        crystalsExtra_Green += crystalsPerSecond_Green;
        crystalsExtra_Yellow += crystalsPerSecond_Yellow;

        //Check if values are over 1
        if (crystalsExtra_Red >= 1)
        {
            crystalsOwned_Red++;
            crystalsExtra_Red--;
        }
        if (crystalsExtra_Blue >= 1)
        {
            crystalsOwned_Blue++;
            crystalsExtra_Blue--;
        }
        if (crystalsExtra_Green >= 1)
        {
            crystalsOwned_Green++;
            crystalsExtra_Green--;
        }
        if (crystalsExtra_Yellow >= 1)
        {
            crystalsOwned_Yellow++;
            crystalsExtra_Yellow--;
        }


        UpdateCrystalUI();

        yield return new WaitForSeconds(1);

        GameOverScript gos = (GameOverScript)FindObjectOfType(typeof(GameOverScript));

        if (!gos.isGameOver)
        {
            //Restart the crystal methods
            StartCoroutine(UpdateCrystalValues());
        }
    }


    ///////////////
    /// <summary>
    /// Update Crystal UI elements
    /// </summary>
    ///////////////
    public void UpdateCrystalUI()
    {
        crystalText_Red.text = "Red Gems: " + crystalsOwned_Red;
        crystalText_Blue.text = "Blue Gems: " + crystalsOwned_Blue;
        crystalText_Green.text = "Green Gems: " + crystalsOwned_Green;
        crystalText_Yellow.text = "Yellow Gems: " + crystalsOwned_Yellow;

        //Red
        if (crystalsOwned_Red >= 5)
        {
            crystalText_Red.gameObject.transform.parent.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            crystalText_Red.gameObject.transform.parent.gameObject.GetComponent<Button>().interactable = false;
        }

        //Blue
        if (crystalsOwned_Blue >= 5)
        {
            crystalText_Blue.gameObject.transform.parent.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            crystalText_Blue.gameObject.transform.parent.gameObject.GetComponent<Button>().interactable = false;
        }

        //Green
        if (crystalsOwned_Green >= 5)
        {
            crystalText_Green.gameObject.transform.parent.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            crystalText_Green.gameObject.transform.parent.gameObject.GetComponent<Button>().interactable = false;
        }

        //Yellow
        if (crystalsOwned_Yellow >= 5)
        {
            crystalText_Yellow.gameObject.transform.parent.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            crystalText_Yellow.gameObject.transform.parent.gameObject.GetComponent<Button>().interactable = false;
        }
    }

}
