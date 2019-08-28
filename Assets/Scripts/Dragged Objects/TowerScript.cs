using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

///////////////
/// <summary>
///     
/// TowerScript Controllers the Towers on the game.
/// 
/// </summary>
///////////////

public class TowerScript : MonoBehaviour
{
    [Header("Tower Info")]
    public string towerName;
    public int currentTowerTier;
    public string color;

    [Header("Refferences")]
    public TowerData towerData;
    public TowerRange towerRange;
    public GameObject firingPoint;
    public GameObject towerUIPanel;
    public SpriteRenderer towerSpriteRenderer;

    [Header("Tower UI")]
    public TextMeshProUGUI towerUpgradeText;
    public TextMeshProUGUI towerSellText;

    [Header("Tower Range")]
    public GameObject RangeVisualizer;

    //////////////////////////////////////////////////////////

    private void Start()
    {
        RangeVisualizer.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Raycast Mouse
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(mousePos, Vector2.zero, 10, LayerMask.GetMask("Tower"));

            //if anything is collided
            if (hit2D.collider != null)
            {
                //if correct tower hit.
                if (hit2D.collider == gameObject.GetComponent<Collider2D>())
                {
                    OpenTowerUI();
                    RangeVisualizer.SetActive(true);
                }
                else
                {
                    CloseTowerUI();
                    RangeVisualizer.SetActive(false);
                }
            }

        }
        else if (Input.GetMouseButtonDown(1))
        {
            CloseTowerUI();
            RangeVisualizer.SetActive(false);
        }

    }

    ////////////////////////////////////////////////////////// - UI Buttons

    public void UpgradeTower()
    {
        //Get Player
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        //Get the sell price
        int upgradePrice = GetUpgradePrice();

        //Check if action is allowed
        if (currentTowerTier < 3)
        { 

            //Charge player for the tower
            if (color == "Red")
            {
                //if (playerStats.crystalsOwned_Red > 4)
                //{
                playerStats.crystalsOwned_Red -= upgradePrice;
                //}
            }
            else if (color == "Blue")
            {
                playerStats.crystalsOwned_Blue -= upgradePrice;
            }
            else if (color == "Green")
            {
                playerStats.crystalsOwned_Green -= upgradePrice;
            }
            else if (color == "Yellow")
            {
                playerStats.crystalsOwned_Yellow -= upgradePrice;
            }


            //Update Values
            currentTowerTier++;
            towerRange.GetTowerData();

            //Get New Sprite
            switch (currentTowerTier)
            {
                case 0:
                    Debug.Log("Error");
                    break;

                case 1:
                    towerSpriteRenderer.sprite = towerData.towerSprite_T1;
                    break;

                case 2:
                    towerSpriteRenderer.sprite = towerData.towerSprite_T2;
                    break;

                case 3:
                    towerSpriteRenderer.sprite = towerData.towerSprite_T3;
                    break;
            }
        }


        //Refresh UI Text
        OpenTowerUI();
    }

    public void SellTower()
    {
        //Get Player
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        //Get the sell price
        int sellPrice = GetSellPrice();

        //Refund player for the tower
        if (color == "Red")
        {
            playerStats.crystalsOwned_Red += sellPrice;
        }
        else if (color == "Blue")
        {
            playerStats.crystalsOwned_Blue += sellPrice;
        }
        else if (color == "Green")
        {
            playerStats.crystalsOwned_Green += sellPrice;
        }
        else if (color == "Yellow")
        {
            playerStats.crystalsOwned_Yellow += sellPrice;
        }

        //Destory the gameobject
        Destroy(gameObject);
    }

    //////////////////////////////////////////////////////////

    public int GetSellPrice()
    {
        int sellPrice = 0;

        //Get Tiered Sell Price
        switch (currentTowerTier)
        {
            case 0:
                Debug.Log("Error");
                break;

            case 1:
                sellPrice = towerData.towerSellPrice_T1;
                break;

            case 2:
                sellPrice = towerData.towerSellPrice_T2;
                break;

            case 3:
                sellPrice = towerData.towerSellPrice_T3;
                break;
        }

        return sellPrice;
    }

    public int GetUpgradePrice()
    {
        int upgradePrice = 0;

        //Get Tiered Sell Price
        switch (currentTowerTier)
        {
            case 0:
                Debug.Log("Error");
                break;

            case 1:
                upgradePrice = towerData.towerUpgradePrice_T1;
                break;

            case 2:
                upgradePrice = towerData.towerUpgradePrice_T2;
                break;

            case 3:
                upgradePrice = towerData.towerUpgradePrice_T3;
                break;
        }

        return upgradePrice;
    }

    ////////////////////////////////////////////////////////// - UI Features

    private void CloseTowerUI()
    {
        towerUIPanel.SetActive(false);
    }

    private void OpenTowerUI()
    {
        towerUIPanel.SetActive(true);

        //LOAD TOWER DATA ONTO UI
        towerUpgradeText.text = GetUpgradePrice().ToString();
        towerSellText.text = GetSellPrice().ToString();
    }

    //////////////////////////////////////////////////////////

    /*


    private void TowerUI()
    {
        if (SelectedTower != null)
        {
            if (CurrentTowerWindow == null)
            {
                CurrentTowerWindow = Instantiate(TowerWindowPrefab, SelectedTower.transform.position, new Quaternion());
                CurrentTowerWindow.GetComponent<TowerNodeUIScript>().changeNodeText(
                    SelectedTower.towerName + " \n" +
                    "Damage: " + SelectedTower.towerRange.currentProjectileData.projectileDamage + "  \n" +
                    "Attack Speed: " + SelectedTower.towerRange.timeToReload);
                //Debug.Log(SelectedTower.projectileData.name + " \n" + SelectedTower.projectileData.projectileDamage + "  \n" + SelectedTower.timeToReload);
            }
        }
        if (SelectedTower == null)
        {
            Destroy(CurrentTowerWindow);
        }
    }


    //////////////////////////////////////////////////////////

    */

}
