using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TileNodes;

[CreateAssetMenu(fileName="NewWave", menuName= "Scriptable Objects/Wave")]
public class WaveData : ScriptableObject
{

    [Header("Accepted enemy type")]
    public GameObject[] AcceptedTypes;

    [Header("Enemy Prefab")]
    public GameObject EnemyPrefab;

    [Header("Enemy per base")]
    public int NumberOfEnemyPerPos; //Numver of enemy per position

    [Header("How Long The Interwave Lasts")]
    public float TimeUntilSpawn; //Count down timer until next wave

    [Header("How Long The Wave Spawning Lasts")]
    public float WaveTimer; //Timer for current wave

    [Header("How Long Until Next Unit Spawn")]
    public float SpawnRate; //How many enemy should be spawn after some time

    private GameObject[] EnemiesPrefab;
    private PathWrapper listWapper;

    //Add node to list will set value of SpawnPosition
    [HideInInspector]
    public List<WorldTile> Path;
    [HideInInspector]
    public List<List<WorldTile>> Paths;
    [HideInInspector]
    public GameObject ParentGameobject;

    public List<WorldTile> SelectedSpawnTiles;


    /////////////////////////////////////////////////////////////////

    /// <summary>
    ///Constructor with parameter for spawning one type of enemy
    /// </summary>
    /// <param name="enemyPrefab">Enemy prefab that will be spawned</param>
    /// <param name="spawnPosition">List of List positions that the enemy will follow</param>
    /// <param name="parentGameobject">Parent to store spawned enemy</param>
    /// <param name="numberOfEnemyPerPos">Amount of enemy that can spawn in a specific position</param>
    public WaveData(GameObject enemyPrefab, List<List<WorldTile>> paths, GameObject parentGameobject, int numberOfEnemyPerPos)
    {
        EnemyPrefab = enemyPrefab;
        ParentGameobject = parentGameobject;
        NumberOfEnemyPerPos = numberOfEnemyPerPos;
        Paths = paths;
    }


    /// <summary>
    ///Constructor with parameter for spawning more than 1 type of enemy
    /// </summary>
    /// <param name="enemyPrefabs">Enemy prefab that will be spawned</param>
    /// <param name="paths">List of List positions that the enemy will follow</param>
    /// <param name="parentGameobject">Parent to store spawned enemy</param>
    /// <param name="numberOfEnemyPerPos">Amount of enemy that can spawn in a specific position</param>
    public WaveData(GameObject[] enemyPrefabs, List<List<WorldTile>> paths, GameObject parentGameobject, int numberOfEnemyPerPos)
    {
        EnemiesPrefab = enemyPrefabs;
        Paths = paths;
        ParentGameobject = parentGameobject;
        NumberOfEnemyPerPos = numberOfEnemyPerPos;
    }

    /// <summary>
    /// Empty constructor
    /// </summary>
    public WaveData()
    {

    }


    public List<WorldTile> GetPath()
    {
        if(Path != null)
        {
            return Path;
        }
        else if(listWapper != null && listWapper.selectedPath != null)
        {
            return listWapper.selectedPath;
        }
        else if(Paths != null && Paths[0] != null)
        {
            return Paths[0];
        }
        else
        {
            Debug.LogError("No Path was found for " + name);
            return new List<WorldTile>();
        }
    }

    public List<WorldTile> GetPath(int i = 0)
    {
        if (Paths != null && i >= 0 && i < Paths.Count && Paths[i] != null)
        {
            return Paths[i];
        }
        else if (Path != null)
        {
            return null;
        }
        else if (listWapper != null && listWapper.selectedPath != null)
        {
            return listWapper.selectedPath;
        }
        else
        {
            Debug.LogError("No Path was found for " + name);
            return new List<WorldTile>();
        }
    }
    
}
