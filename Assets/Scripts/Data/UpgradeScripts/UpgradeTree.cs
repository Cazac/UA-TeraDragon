using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewUpgradeTree", menuName = "Scriptable Objects/Upgrade Tree")]
public class UpgradeTree : ScriptableObject {
    
    public UpgradeNode[] tree;

    public float GetTotalStatsUpgrade(StatsCategory category) {
        float total = 0f;
        foreach (UpgradeNode upgradeNode in tree) {
            if (upgradeNode.GetUpgradeType() == UpgradeType.STATS_MODIFIER && upgradeNode.isActive) {
                if(((StatsUpgrade)upgradeNode).statsCategory == category) {
                    total += ((StatsUpgrade)upgradeNode).StatsMultiplier;
                }
            }
        }
        return total;
    }

    public List<string> GetActivateAbilities() {
        List<string> activeAbilities = new List<string>();

        foreach (UpgradeNode upgradeNode in tree) {
            if (upgradeNode.GetUpgradeType() == UpgradeType.EFFECT && upgradeNode.isActive) {
                activeAbilities.Add(((EffectUpgrade)upgradeNode).effectUpgrade);
            }
        }
        return activeAbilities;
    }
    
}
