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

    [Header("Skill Fillbars UI")]
    public GameObject skillFillGameObject_Red;
    public GameObject skillFillGameObject_Blue;
    public GameObject skillFillGameObject_Green;
    public GameObject skillFillGameObject_Yellow;

    [Header("Miners")]
    public int minersOwned;

    [Header("SFX")]
    public SoundObject hitSFX;
    public SoundObject gemSFX;

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
    public float skillCountdown_Red;
    public float skillCountdown_Blue;
    public float skillCountdown_Green;
    public float skillCountdown_Yellow;

    //STATIC VALUES WHERE TO CHANGE???
    private float skillCooldown_Red = 20f;
    private float skillCooldown_Blue = 15f;
    private float skillCooldown_Green = 25f;
    private float skillCooldown_Yellow = 15f;

    private GameOverScript gameOverScript;
    private SoundManager soundManager;

    [Header("Sprite Prefabs")]
    public GameObject hpSprite_Prefab;
    public GameObject levelSprite_Prefab;

    [Header("Sprite Sheets")]
    public Sprite[] hpSpriteSheet;
    public Sprite[] levelSpriteSheet;

    [Header("Base Node")]
    public GameObject baseNode;
    private GameObject hpSprite;
    private GameObject levelSprite;

    public int currentWaveCounter = 0;

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

        if (levelSpriteSheet.Length >= currentWaveCounter + 1)
        {
            levelSprite.GetComponent<SpriteRenderer>().sprite = levelSpriteSheet[currentWaveCounter];
        }
    }

    public void RemoveLife(int i)
    {
        CurrentLives -= i;

        //SFX
        soundManager.PlayOnUIClick(hitSFX, 0);

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
        print("Test Code: BLANK");
        soundManager.PlaySpecificSound("Death");
    }

    public void AddCrystal(CrystalColor color)
    {
        //SFX
        soundManager.PlayOnUIClick(gemSFX, 0.1f);


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
    /// Update the counters on the skills cooldown, The fill bars are using a range of 60 - 0
    /// </summary>
    ///////////////
    public void UpdateSkillCooldowns()
    {
        if (!skillReady_Red)
        {
            skillCountdown_Red += Time.deltaTime;

            //Set Ratio For Bar
            float ratio = (skillCountdown_Red / skillCooldown_Red);
            ratio = ratio * 60;
            ratio = ratio - 60;

            if (ratio > 0f)
            {
                ratio = 0f;
            }

            //Update Bar
            skillFillGameObject_Red.GetComponent<RectTransform>().offsetMax = new Vector2(ratio, 0); 

            if (skillCountdown_Red >= skillCooldown_Red)
            {
                skillReady_Red = true;
                skillCountdown_Red = 0;
            }
        }
        else
        {
            skillCountdown_Red = 0;
        }

        if (!skillReady_Blue)
        {
            skillCountdown_Blue += Time.deltaTime;

            //Set Ratio For Bar
            float ratio = (skillCountdown_Blue / skillCooldown_Blue);
            ratio = ratio * 60;
            ratio = ratio - 60;

            if (ratio > 0f)
            {
                ratio = 0f;
            }

            //Update Bar
            skillFillGameObject_Blue.GetComponent<RectTransform>().offsetMax = new Vector2(ratio, 0);

            if (skillCountdown_Blue >= skillCooldown_Blue)
            {
                skillReady_Blue = true;
                skillCountdown_Blue = 0;
            }
        }
        else
        {
            skillCountdown_Blue = 0;
        }

        if (!skillReady_Green)
        {
            skillCountdown_Green += Time.deltaTime;

            //Set Ratio For Bar
            float ratio = (skillCountdown_Green / skillCooldown_Green);
            ratio = ratio * 60;
            ratio = ratio - 60;

            if (ratio > 0f)
            {
                ratio = 0f;
            }

            //Update Bar
            skillFillGameObject_Green.GetComponent<RectTransform>().offsetMax = new Vector2(ratio, 0);

            if (skillCountdown_Green >= skillCooldown_Green)
            {
                skillReady_Green = true;
                skillCountdown_Green = 0;
            }
        }
        else
        {
            skillCountdown_Green = 0;
        }

        if (!skillReady_Yellow)
        {
            skillCountdown_Yellow += Time.deltaTime;

            //Set Ratio For Bar
            float ratio = (skillCountdown_Yellow / skillCooldown_Yellow);
            ratio = ratio * 60;
            ratio = ratio - 60;

            if (ratio > 0f)
            {
                ratio = 0f;
            }

            //Update Bar
            skillFillGameObject_Yellow.GetComponent<RectTransform>().offsetMax = new Vector2(ratio, 0);

            if (skillCountdown_Yellow >= skillCooldown_Yellow)
            {
                skillReady_Yellow = true;
                skillCountdown_Yellow = 0;
            }
        }
        else
        {
            skillCountdown_Yellow = 0;
        }
    }

    ///////////////
    /// <summary>
    /// Update Crystal UI elements
    /// </summary>
    ///////////////
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

    ///////////////
    /// <summary>
    /// Update Miner UI elements
    /// </summary>
    ///////////////
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
