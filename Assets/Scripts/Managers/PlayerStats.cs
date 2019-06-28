using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public int MaxLives;
    public int CurrentLives;

    [Header("Crystal UI ELements")]
    public Text crystalText_Red;
    public Text crystalText_Blue;
    public Text crystalText_Green;
    public Text crystalText_Yellow;

    [Header("Skill UI Elements")]
    public GameObject skillGameObject_Red;

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

    public bool skillReady_Red;

    private float skillCountdown_Red;

    private float skillCooldown_Red = 5f;

    /////////////////////////////////////////////////////////////////

    private void Start()
    {
        CurrentLives = MaxLives;

        //Start the crystal chekcer
        StartCoroutine(UpdateCrystalValues());
    }

    private void Update()
    {
        UpdateSkillCooldowns();
        UpdateSkillUI();
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

    public void AddCrystal(CrystalColor color)
    {
        switch (color)
        {
            case CrystalColor.RED:
                crystalsOwned_Red++;
                return;
            case CrystalColor.BLUE:
                crystalsOwned_Blue++;
                return;
            case CrystalColor.GREEN:
                crystalsOwned_Green++;
                return;
            case CrystalColor.YELLOW:
                crystalsOwned_Yellow++;
                return;
        }
    }





    ///////////////
    /// <summary>
    /// Undocumented
    /// </summary>
    ///////////////
    public void UpdateSkillCooldowns()
    {
        if (!skillReady_Red)
        {
            skillCountdown_Red += Time.deltaTime;

            if (skillCountdown_Red >= skillCooldown_Red)
            {
                skillReady_Red = true;
                skillCountdown_Red = 0;
            }
        }
    }

    /// <summary>
    /// Undocumented
    /// </summary>
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

    /// <summary>
    /// Update Crystal UI elements
    /// </summary>
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


    ///////////////
    /// <summary>
    /// Update Crystal UI elements
    /// </summary>
    ///////////////
    public void UpdateSkillUI()
    {
        //Red
        if (skillReady_Red)
        {
            skillGameObject_Red.gameObject.transform.GetComponent<Button>().interactable = true;
        }
        else
        {
            skillGameObject_Red.gameObject.transform.GetComponent<Button>().interactable = false;
        }

    }

}
