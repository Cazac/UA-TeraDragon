using UnityEngine;
using System.Collections;
using System;

public class WaveSystem : MonoBehaviour
{

    public float timeUntilSpawn;

    public GameObject[] spawnEnemies;

    public Boolean spawnButton;

    [SerializeField]
    private float internalTimer;


    private void Start() 
    {
        
    }

    private void MakeParent()
    {

    }

    private void LateUpdate() 
    {

    }


    
    public void SpawnSingleEnemyWorldPos(GameObject enemyPrefab, Vector3 position, GameObject parentGameobject)
    {
        Instantiate(enemyPrefab, position, Quaternion.identity, parentGameobject.transform);
    }

    // private bool InternalTimerCountdown()
    // {
    //     internalTimer += Time.deltaTime;
    //     if(internalTimer >= timeUntilSpawn)
    //         return true;

    // }


}