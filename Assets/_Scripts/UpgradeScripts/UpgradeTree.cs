using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewUpgradeTree", menuName = "ScriptableObjects/Upgrade Tree")]
public class UpgradeTree : ScriptableObject {
    
    public UpgradeNode[] tree;

    
}
