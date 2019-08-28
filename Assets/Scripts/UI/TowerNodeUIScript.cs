using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerNodeUIScript : MonoBehaviour
{
    //////////////////////////////////////////////////////////
    
    [Header("Main Script")]
    public TowerScript towerScript;

    [Header("Tower Buttons")]
    public Button upgradeButton;
    public Button sellButton;

    [Header("Tower Text")]
    public Text towerUpgradeText;
    public Text towerSellText;

    //////////////////////////////////////////////////////////
   
    private void Update()
    {
        CheckButtonActive();
    }

    //////////////////////////////////////////////////////////

    private void CheckButtonActive()
    {

        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        int upgradePrice = towerScript.GetUpgradePrice();
        string color = towerScript.color;
        int crystalAmount = 0;

        //Refund player for the tower
        if (color == "Red")
        {
            crystalAmount = playerStats.crystalsOwned_Red;
        }
        else if (color == "Blue")
        {
            crystalAmount = playerStats.crystalsOwned_Blue;
        }
        else if (color == "Green")
        {
            crystalAmount = playerStats.crystalsOwned_Green;
        }
        else if (color == "Yellow")
        {
            crystalAmount = playerStats.crystalsOwned_Yellow;
        }


        if (crystalAmount >= upgradePrice)
        {
            //Enable
            upgradeButton.interactable = true;
        }
        else
        {
            //Disable
            upgradeButton.interactable = false;
        }
    }

    //////////////////////////////////////////////////////////
}
