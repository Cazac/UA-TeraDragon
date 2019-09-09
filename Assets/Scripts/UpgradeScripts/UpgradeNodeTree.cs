using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class UpgradeNodeTree : ScriptableObject {
    public string upgradeName;
    private UpgradeType upgradeType;
    public bool isActive;
    public UpgradeNodeTree[] necessaryNodes;

    public void ActivateUpgrade() {
        foreach(UpgradeNodeTree node in necessaryNodes) {
            if (!node.isActive)
                return;
        }
        isActive = true;
    }
    
    public UpgradeType GetUpgradeType() {
        return upgradeType;
    }
    
}

public enum UpgradeType {
    STATS_MODIFIER,
    EFFECT,
}
