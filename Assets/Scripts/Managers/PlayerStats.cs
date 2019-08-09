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

    [Header("Miners UI ELements")]
    public Text minerText;

    [Header("Skill UI Elements")]
    public GameObject skillGameObject_Red;
    public GameObject skillGameObject_Blue;
    public GameObject skillGameObject_Green;
    public GameObject skillGameObject_Yellow;

    [Header("Miners")]
    public int minersOwned;

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
    private SoundManager soundManager;

    [Header("Sprite Prefabs")]
    public GameObject hpSprite_Prefab;
    public GameObject levelSprite_Prefab;

    [Header("Sprite Sheets")]
    public Sprite[] hpSpriteSheet;
    public Sprite[] levelSpriteSheet;

    private GameObject baseNode;
    private GameObject hpSprite;
    private GameObject levelSprite;

    private int currentWaveCounter = 0;

    [Header("Text Prefabs")]
    public GameObject popupDamageText_Prefab;
    public GameObject popupGemText_Prefab;

    /////////////////////////////////////////////////////////////////

    private void Start()
    {
        CurrentLives = MaxLives;

        gameOverScript = GameObject.FindObjectOfType<GameOverScript>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();

        //Get Base Node and make Sprite prefabs
        baseNode = GameObject.Find("NODE 0 : 8");
        hpSprite = Instantiate(hpSprite_Prefab, baseNode.transform.position, Quaternion.identity, baseNode.transform);
        levelSprite = Instantiate(levelSprite_Prefab, baseNode.transform.position, Quaternion.identity, baseNode.transform);

        //Move up the HP 
        levelSprite.transform.position = new Vector3(levelSprite.transform.position.x - 3.2f, levelSprite.transform.position.y + 1.4f, -50);

        //Set Base Sprites
        hpSprite.GetComponent<SpriteRenderer>().sprite = hpSpriteSheet[CurrentLives];
        levelSprite.GetComponent<SpriteRenderer>().sprite = levelSpriteSheet[currentWaveCounter];
    }

    private void Update()
    {
        UpdateCrystalUI();
        UpdateSkillCooldowns();
        UpdateSkillUI();
        UpdateMinerUI();
    }

    /////////////////////////////////////////////////////////////////

    public void IncrementWaveSprite()
    {
        currentWaveCounter++;
        levelSprite.GetComponent<SpriteRenderer>().sprite = levelSpriteSheet[currentWaveCounter];
    }

    public void RemoveLife(int i)
    {
        CurrentLives -= i;
        //Debug.Log("Hit! Lose " + i + " lives, Current Lives:" + CurrentLives);

        if (CurrentLives >= 0)
        {
            hpSprite.GetComponent<SpriteRenderer>().sprite = hpSpriteSheet[CurrentLives];
        }

        if (CurrentLives < 0)
        {
            CurrentLives = 0;
        }
        else
        {
            
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
        //Debug.Log("Game over man, Game over");
        gameOverScript.TurnOnGameOver();
        soundManager.PlaySpecificSound("Death");
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
        crystalText_Red.text = crystalsOwned_Red.ToString();
        crystalText_Blue.text = crystalsOwned_Blue.ToString();
        crystalText_Green.text = crystalsOwned_Green.ToString();
        crystalText_Yellow.text = crystalsOwned_Yellow.ToString();

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

    /// <summary>
    /// Update Crystal UI elements
    /// </summary>
    public void UpdateMinerUI()
    {
        minerText.text = minersOwned.ToString();
  
        //Miners
        if (minersOwned >= 1)
        {
            minerText.gameObject.transform.parent.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            minerText.gameObject.transform.parent.gameObject.GetComponent<Button>().interactable = false;
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

        //Blue
        if (skillReady_Blue)
        {
            skillGameObject_Blue.gameObject.transform.GetComponent<Button>().interactable = true;
        }
        else
        {
            skillGameObject_Blue.gameObject.transform.GetComponent<Button>().interactable = false;
        }

        //Green
        if (skillReady_Green)
        {
            skillGameObject_Green.gameObject.transform.GetComponent<Button>().interactable = true;
        }
        else
        {
            skillGameObject_Green.gameObject.transform.GetComponent<Button>().interactable = false;
        }

        //Yellow
        if (skillReady_Yellow)
        {
            skillGameObject_Yellow.gameObject.transform.GetComponent<Button>().interactable = true;
        }
        else
        {
            skillGameObject_Yellow.gameObject.transform.GetComponent<Button>().interactable = false;
        }

    }

}
