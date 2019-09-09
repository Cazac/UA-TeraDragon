using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManagerUILogic : MonoBehaviour
{
    public Text[] numOfCrystalsTexts;
    public Text[] upgradeCostTexts;
    public TextMeshProUGUI moneyText;

    private PlayerStats playerStats;
    private UpgradeManager upgradeManager;

    private void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        upgradeManager = FindObjectOfType<UpgradeManager>();
    }
    private void Update()
    {
        UpdateNumCrystalTexts();
        UpdateUpgradeCostTexts();
        moneyText.text = "Money: " + upgradeManager.upgradeMoney;
    }

    private void UpdateNumCrystalTexts()
    {
        numOfCrystalsTexts[0].text = "Number of red crystals: " + playerStats.crystalsOwned_Red;
        numOfCrystalsTexts[1].text = "Number of blue crystals: " + playerStats.crystalsOwned_Blue;
        numOfCrystalsTexts[2].text = "Number of green crystals: " + playerStats.crystalsOwned_Green;
        numOfCrystalsTexts[3].text = "Number of yellow crystals: " + playerStats.crystalsOwned_Yellow;
    }

    private void UpdateUpgradeCostTexts()
    {
        upgradeCostTexts[0].text = "Upgrade Cost: " + upgradeManager.redTowerUpgradeCost;
        upgradeCostTexts[1].text = "Upgrade Cost: " + upgradeManager.blueTowerUpgradeCost;
        upgradeCostTexts[2].text = "Upgrade Cost: " + upgradeManager.greenTowerUpgradeCost;
        upgradeCostTexts[3].text = "Upgrade Cost: " + upgradeManager.yellowTowerUpgradeCost;
    }

}
