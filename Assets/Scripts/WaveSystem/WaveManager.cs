using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public float timeUntilSpawn;

    public GameObject[] spawnEnemies;
    public GameObject spawnSingleEnemy;

    public bool enableSpawning;
    public Vector3[] positions;
    public List<Vector3> NodeSpawnPosition { get => nodeSpawnPosition; set => nodeSpawnPosition = value; }

    [SerializeField]
    private float internalTimer;

    [Header("Scriptable wave object")]
    public Wave currentWave;

    [SerializeField] //TODO: Delete serializeField
    private List<Vector3> nodeSpawnPosition = new List<Vector3>();
    private const string WAVE_PARENT_NAME = "Parent_EnemyWave";

    private Timer timer;

    private void Start() 
    {
       MakeParent();
    }

    private void Update() 
    {
       if(enableSpawning)
       {
            if(currentWave != null)
            {
                currentWave = new Wave(spawnSingleEnemy, NodeSpawnPosition, GameObject.Find(WAVE_PARENT_NAME), timeUntilSpawn);
                StartCoroutine( SpawnPerWave(currentWave));
            }
            
            Debug.Log(currentWave.ToString());
       }

    }

    /// <summary>
    ///Create new parent gameobject to store enemy info
    /// </summary>
    private void MakeParent()
    {
         Instantiate(new GameObject(WAVE_PARENT_NAME));
    }

    private void LateUpdate() 
    {
        
    }
    
    public void SpawnEnemyWorldPos(GameObject enemyPrefab, Vector3 spawnPosition, GameObject parentGameobject)
    {
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, parentGameobject.transform);
    }
    
    public IEnumerator SpawnPerWave(Wave wave)
    {
        Debug.Log("SPawning in manager");
        wave.ParentGameobject = GameObject.Find(WAVE_PARENT_NAME);
        foreach (var position in wave.SpawnPosition)
        {
            Instantiate(wave.EnemyPrefab, position, Quaternion.identity, wave.ParentGameobject.transform);
            yield return new WaitForSeconds(timeUntilSpawn);
        }
    }
}