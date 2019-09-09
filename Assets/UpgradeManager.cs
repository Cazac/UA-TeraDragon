using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class UpgradeManager : MonoBehaviour
{
    public int upgradeMoney;

    [Header("Tower upgrade level")]
    public int redTowerUpgradeLevel;
    public int blueTowerUpgradeLevel;
    public int greenTowerUpgradeLevel;
    public int yellowTowerUpgradeLevel;

    [Header("Tower upgrade cost")]
    public int redTowerUpgradeCost;
    public int blueTowerUpgradeCost;
    public int greenTowerUpgradeCost;
    public int yellowTowerUpgradeCost;

    PlayerStats playerStats;

    private void Start()
    {
        playerStats = GameObject.FindObjectOfType<PlayerStats>();
        redTowerUpgradeCost = 1;
        blueTowerUpgradeCost = 1;
        greenTowerUpgradeCost = 1;
        yellowTowerUpgradeCost = 1;
    }

    public void ConverteCyrstalToMoney(int color)
    {

        switch( color )
        {
            case 1:
                if( playerStats.crystalsOwned_Blue > 0)
                {
                    playerStats.crystalsOwned_Blue--;
                    upgradeMoney++;
                }
                break;
            case 2:
                if (playerStats.crystalsOwned_Green > 0)
                {
                    playerStats.crystalsOwned_Green--;
                    upgradeMoney++;
                }
                break;
            case 0:
                if (playerStats.crystalsOwned_Red > 0)
                {
                    playerStats.crystalsOwned_Red--;
                    upgradeMoney++;
                }
                break;
            case 3:
                if (playerStats.crystalsOwned_Yellow > 0)
                {
                    playerStats.crystalsOwned_Yellow--;
                    upgradeMoney++;
                }
                break;
        }
    }

    public void IncreaseTowerUpgradeLevel(int color)
    {
        switch (color)
        {
            case 1:
                if (upgradeMoney >= blueTowerUpgradeCost)
                {
                    upgradeMoney -= blueTowerUpgradeCost;
                    blueTowerUpgradeCost++;
                    blueTowerUpgradeLevel++;
                }
                break;
            case 2:
                if (upgradeMoney >= greenTowerUpgradeCost)
                {
                    upgradeMoney -= greenTowerUpgradeCost;
                    greenTowerUpgradeCost++;
                    greenTowerUpgradeLevel++;
                }
                break;
            case 0:
                if (upgradeMoney >= redTowerUpgradeCost)
                {
                    upgradeMoney -= redTowerUpgradeCost;
                    redTowerUpgradeCost++;
                    redTowerUpgradeLevel++;
                }
                break;
            case 3:
                if (upgradeMoney >= yellowTowerUpgradeCost)
                {
                    upgradeMoney -= yellowTowerUpgradeCost;
                    yellowTowerUpgradeCost++;
                    yellowTowerUpgradeLevel++;
                }
                break;
        }
    }


}
