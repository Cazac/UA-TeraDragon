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
    public GameObject skillGameObject_Blue;
    public GameObject skillGameObject_Green;
    public GameObject skillGameObject_Yellow;

    [Header("Crystals")]
    public int crystalsOwned_Red;
    public int crystalsOwned_Blue;
    public int crystalsOwned_Green;
    public int crystalsOwned_Yellow;

    //Skill ready values
    public bool skillReady_Red;
    public bool skillReady_Blue;
    public bool skillReady_Green;
    public bool skillReady_Yellow;

    //Skill ready to recharge values
    //public bool skillReady_Red;
    //public bool skillReady_Blue;
   // public bool skillReady_Green;
    //public bool skillReady_Yellow;

    //Skill countdown values
    private float skillCountdown_Red;
    private float skillCountdown_Blue;
    private float skillCountdown_Green;
    private float skillCountdown_Yellow;

    //STATIC VALUES WHERE TO CHANGE???
    private float skillCooldown_Red = 10f;
    private float skillCooldown_Blue = 5f;
    private float skillCooldown_Green = 5f;
    private float skillCooldown_Yellow = 5f;

    private GameOverScript gameOverScript;

    /////////////////////////////////////////////////////////////////

    private void Start()
    {
        CurrentLives = MaxLives;

        gameOverScript = GameObject.FindObjectOfType<GameOverScript>();
        //Start the crystal chekcer
        //StartCoroutine(UpdateCrystalValues());
    }

    private void Update()
    {
        UpdateCrystalUI();
        UpdateSkillCooldowns();
        UpdateSkillUI();
    }

    /////////////////////////////////////////////////////////////////

    public void RemoveLife(int i)
    {
        CurrentLives -= i;
        Debug.Log("Hit! Lose " + i + " lives, Current Lives:" + CurrentLives);

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
        Debug.Log("Game over man, Game over");
        gameOverScript.TurnOnGameOver();
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
    /// Update the counters on the skills cooldown
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

        if (!skillReady_Blue)
        {
            skillCountdown_Blue += Time.deltaTime;

            if (skillCountdown_Blue >= skillCooldown_Blue)
            {
                skillReady_Blue = true;
                skillCountdown_Blue = 0;
            }
        }

        if (!skillReady_Green)
        {
            skillCountdown_Green += Time.deltaTime;

            if (skillCountdown_Green >= skillCooldown_Green)
            {
                skillReady_Green = true;
                skillCountdown_Green = 0;
            }
        }

        if (!skillReady_Yellow)
        {
            skillCountdown_Yellow += Time.deltaTime;

            if (skillCountdown_Yellow >= skillCooldown_Yellow)
            {
                skillReady_Yellow = true;
                skillCountdown_Yellow = 0;
            }
        }
    }

    /// <summary>
    /// Undocumented
    /// </summary>
    /*
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
    */

    /// <summary>
    /// Update Crystal UI elements
    /// </summary>
    public void UpdateCrystalUI()
    {
        crystalText_Red.text = "Gems: " + crystalsOwned_Red;
        crystalText_Blue.text = "Gems: " + crystalsOwned_Blue;
        crystalText_Green.text = "Gems: " + crystalsOwned_Green;
        crystalText_Yellow.text = "Gems: " + crystalsOwned_Yellow;

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




        //THESE ARE ALL DISABLED
        //THESE ARE ALL DISABLED
        //THESE ARE ALL DISABLED
        //THESE ARE ALL DISABLED
        //THESE ARE ALL DISABLED


        //Yellow
        if (crystalsOwned_Yellow >= 5)
        {
            crystalText_Yellow.gameObject.transform.parent.gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            crystalText_Yellow.gameObject.transform.parent.gameObject.GetComponent<Button>().interactable = false;
        }
    }


    ///////////////
    /// <summary>
    /// Update Skill UI elements
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





        //THESE ARE ALL DISABLED
        //THESE ARE ALL DISABLED
        //THESE ARE ALL DISABLED
        //THESE ARE ALL DISABLED
        //THESE ARE ALL DISABLED




        //Blue
        if (skillReady_Blue)
        {
            skillGameObject_Blue.gameObject.transform.GetComponent<Button>().interactable = false;
        }
        else
        {
            skillGameObject_Blue.gameObject.transform.GetComponent<Button>().interactable = false;
        }

        //Green
        if (skillReady_Green)
        {
            skillGameObject_Green.gameObject.transform.GetComponent<Button>().interactable = false;
        }
        else
        {
            skillGameObject_Green.gameObject.transform.GetComponent<Button>().interactable = false;
        }

        //Yellow
        if (skillReady_Yellow)
        {
            skillGameObject_Yellow.gameObject.transform.GetComponent<Button>().interactable = false;
        }
        else
        {
            skillGameObject_Yellow.gameObject.transform.GetComponent<Button>().interactable = false;
        }

    }

}
