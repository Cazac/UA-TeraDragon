using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Wave", menuName="WaveSetting")]
public class Wave : ScriptableObject
{
    public GameObject EnemyPrefab;
    public List<Vector3> SpawnPosition;
    [HideInInspector]
    public GameObject ParentGameobject;
    public float TimeUntilSpawn;

    public Wave(GameObject enemyPrefab, List<Vector3> spawnPosition, GameObject parentGameobject, float timeUntilSpawn)
    {
        EnemyPrefab = enemyPrefab;
        SpawnPosition = spawnPosition;
        ParentGameobject = parentGameobject;
        TimeUntilSpawn = timeUntilSpawn;
    }
}
