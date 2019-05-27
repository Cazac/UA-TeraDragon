using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Wave", menuName="WaveSetting")]
public class Wave : ScriptableObject
{
    public GameObject EnemyPrefab;
    public GameObject[] EnemiesPrefab;

    //Add node to list will set value of SpawnPosition
    public List<GameObject> SpawnPositionByNodes;
    public List<Vector3> SpawnPosition;
    [HideInInspector]
    public GameObject ParentGameobject;
    public int NumberOfEnemyPerPos;


    public Wave(GameObject enemyPrefab, List<Vector3> spawnPosition, GameObject parentGameobject, int numberOfEnemyPerPos)
    {
        EnemyPrefab = enemyPrefab;
        SpawnPosition = spawnPosition;
        ParentGameobject = parentGameobject;
        NumberOfEnemyPerPos = numberOfEnemyPerPos;

        foreach (var node in SpawnPositionByNodes)
            SpawnPosition.Add(node.transform.position);
    }

    public Wave(GameObject[] enemyPrefabs, List<Vector3> spawnPosition, GameObject parentGameobject, int numberOfEnemyPerPos)
    {
        EnemiesPrefab = enemyPrefabs;
        SpawnPosition = spawnPosition;
        ParentGameobject = parentGameobject;
        NumberOfEnemyPerPos = numberOfEnemyPerPos;

        foreach (var node in SpawnPositionByNodes)
            SpawnPosition.Add(node.transform.position);
    }
    
}
