using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEditor;

///////////////
/// <summary>
///     
/// Why does WaveSystem have a namespace?
/// 
/// </summary>
///////////////

namespace WaveSystem
{

    ///////////////
    /// <summary>
    ///     
    /// WaveManager
    /// 
    /// </summary>
    ///////////////
    public class WaveManager : MonoBehaviour
    {
        [Header("TileNodes Refference")]
        public TileNodes tiles;


        [Header("Scriptable wave objects")]
        //public WaveData[] waves;
        public SpawnList[] spawnListEditorInstance;
        [SerializeField]
        private SpawnList[] clonedSpawnListObject;

        [Header("DEBUG ONLY currentWave")]
        [SerializeField]
        private WaveData currentWave;

        [Header("Parent Gameobject For Waves")]
        public GameObject waveParent;


        // public GameObject[] spawnEnemies; //TODO: Reimplement this!
        public GameObject spawnSingleEnemy;
        public bool enableSpawning;
        public bool EnableSpawning { get => enableSpawning; set => enableSpawning = value; }


        [SerializeField] //TODO: Delete serializeField
        private List<Vector3> selectedNodeSpawnPosition = new List<Vector3>();
        public List<Vector3> NodeSpawnPosition { get => selectedNodeSpawnPosition; set => selectedNodeSpawnPosition = value; }


        private Timer currentTimer;
        public WaveData CurrentWave { get => currentWave; set => currentWave = value; }
        private int waveIndex = 0;


        private TileNodes tileNodes;
        private CursorSelection cursorSelection;
        private SoundManager soundManager;

        public int WaveIndex { get => waveIndex; }

        //////////////////////////////////////////////////////////

        public String TimeUntilNextWave
        {
            get
            {
                if (currentTimer != null)
                {
                    return currentTimer.TimeUntilNextSpawn.ToString();
                }

                return "0";
            }
        }

        public String WaveTimer
        {
            get
            {
                return currentTimer.WaveTimer.ToString();
            }
        }

        //////////////////////////////////////////////////////////

        private void Awake()
        {
            ReadSavePointsPrefab(ref spawnListEditorInstance);
        }

        private void Start()
        {
            clonedSpawnListObject = (SpawnList[])spawnListEditorInstance.Clone();
            WaveData wave = clonedSpawnListObject[0].wave;
            soundManager = GameObject.FindObjectOfType<SoundManager>();
            tileNodes = GameObject.FindObjectOfType<TileNodes>();
            //Cached CursorSelection
            cursorSelection = GameObject.FindObjectOfType<CursorSelection>();

            SaveSpawnPointsAsPrefab(clonedSpawnListObject);

            AssignCurrentWavePath(clonedSpawnListObject);

            CurrentWave = clonedSpawnListObject[0].wave;

            StartCoroutine(SpawnEnemyPerWave());
        }

        private void Update()
        {
            //Create new timer object for current wave
            if (EnableSpawning == true && currentTimer == null)
            {
                //print("New Timer For: " + CurrentWave.name);
                InstantiateNewTimer(CurrentWave.TimeUntilSpawn, CurrentWave.WaveTimer, ref currentTimer);
            }

            //Debug.Log("Is Current Timer Null? " + currentTimer != null);

            //If timer for a wave hits 0, turn off spawning
            if (currentTimer != null && EnableSpawning == true)
            {
                if (currentTimer.WaveCountdown())
                {
                    EnableSpawning = false;
                }
            }


            //If timer between wave hits 0, turn on spawning
            if (currentTimer != null && EnableSpawning == false && !AllWaveCompleted() && waveParent.transform.childCount <= 0)
            {
                if (currentTimer.NextWaveCountdown())
                {
                    EnableSpawning = true;
                }
            }
        }

        //////////////////////////////////////////////////////////

        /// <summary>
        ///     Create a new timer instance when a wave is finished
        /// </summary>
        /// <param name="timeUntilSpawn">Timer between wave until next wave starts</param>
        /// <param name="waveTimer">Timer for current wave</param>
        /// <param name="timer">ref variable timer to reference the timer instance</param>
        private void InstantiateNewTimer(float timeUntilSpawn, float waveTimer, ref Timer timer)
        {
            //Debug.Log("Timer");
            timer = null;
            timer = new Timer(timeUntilSpawn, waveTimer);

           //Debug.Log("Is Current Timer Null? " + currentTimer != null);
        }

        /// <summary>
        ///     Main method for spawning a single type of enemy in multiple position
        /// </summary>
        /// <remarks>
        ///     Use IEnumerator and must only be called in Start()
        /// </remarks>
        public IEnumerator SpawnEnemyPerWave()
        {
            waveIndex = 0;
            //While loop to keep function running every frame if possible
            while (true)
            {
                //Debug.Log("Wave index: " + waveIndex);
                if (EnableSpawning == true)
                {
                    CurrentWave.ParentGameobject = waveParent;

                    //foreach (List<WorldTile> path in CurrentWave.Paths)
                    //{
                    //    for (int i = 0; i < CurrentWave.NumberOfEnemyPerPos; i++)
                    //    {
                    //        //Enumerator will return at this index, need to check if spawning option is still available
                    //        if (EnableSpawning == true)
                    //        {
                    //            DrawDebugPath(CurrentWave.Paths);

                    //            GameObject enemy = Instantiate(CurrentWave.EnemyPrefab, path[0].transform.position, Quaternion.identity, CurrentWave.ParentGameobject.transform);
                    //            enemy.GetComponent<EnemyScript>().currentWaypoints = path;
                    //            enemy.GetComponent<EnemyScript>().PathRelocation();

                    //            yield return new WaitForSeconds(CurrentWave.SpawnRate);
                    //        }
                    //    }
                    //}

                    foreach (List<WorldTile> path in CurrentWave.Paths)
                    {
                        //Enumerator will return at this index, need to check if spawning option is still available
                        if (EnableSpawning == true)
                        {
                            DrawDebugPath(CurrentWave.Paths);

                            GameObject enemy = Instantiate(CurrentWave.EnemyPrefab, path[0].transform.position, Quaternion.identity, CurrentWave.ParentGameobject.transform);
                            enemy.GetComponent<EnemyScript>().currentWaypoints = path;
                            enemy.GetComponent<EnemyScript>().PathRelocation();

                            yield return new WaitForSeconds(CurrentWave.SpawnRate);
                        }


                    }

                    //EnableSpawning = false;
                }

                else
                {
                    //Move on to the next scriptable wave in waves array
                    if (!AllWaveCompleted() && currentTimer.TimeUntilNextSpawn <= 0)
                    {
                        CurrentWave = null;

                        //Double Check
                        if (clonedSpawnListObject.Length >= waveIndex + 1)
                        {
                            CurrentWave = clonedSpawnListObject[++waveIndex].wave;
                        }
                        else
                        {
                            Debug.Log("NOOOPE");
                        }
                 
                        InstantiateNewTimer(CurrentWave.TimeUntilSpawn, CurrentWave.WaveTimer, ref currentTimer);

                        EnableSpawning = true;

                        if (soundManager != null)
                        {

                            //ENDING A WAVE SPAWNING

                            //soundManager.ReturnControl = false;

                            //Debug.Log("Interwave");

                            //soundManager.PlaySpecificSound("Inter");
                        }
                    }

                    else if (AllWaveCompleted())
                    {
                        EnableSpawning = false;
                        StopCoroutine(SpawnEnemyPerWave());
                    }
                }


                yield return null;
            }
        }

        /// <summary>
        ///Check if all waves in waves[] have been processed
        /// </summary>
        ///<returns>Returns true if reaches the end of array</returns>
        private Boolean AllWaveCompleted()
        {
 
            if (waveIndex > clonedSpawnListObject.Length - 1)
            {
                //Debug.Log("End of all waves");

                Winner();

                return true;
            }

            return false;
        }


        private void Winner()
        {
            WinnerScript winner = GameObject.FindObjectOfType<WinnerScript>();
            winner.TurnOnWinner();
        }

        private void DrawDebugPath(List<List<WorldTile>> pathData)
        {
            foreach (var path in pathData)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawRay(path[i].transform.position, path[i + 1].transform.position - path[i].transform.position, Color.white, 100f, false);
                }
            }
        }

        /// <summary>
        /// Assign path of a wave using list from SpawnList object
        /// </summary>
        /// <param name="spawnList">parameter for SpawnList object list</param>
        private void AssignCurrentWavePath(SpawnList[] spawnList)
        {
            for (int i = 0; i < spawnList.Length; i++)
            {
                if (spawnList[i].spawnTileList.Count <= 0)
                    spawnList[i].wave.Paths = new List<List<WorldTile>>() { tiles.pathData.paths[i] };
                else
                {
                    if (spawnList[i].wave.Paths == null)
                        spawnList[i].wave.Paths = new List<List<WorldTile>>();

                    foreach (WorldTile worldTile in spawnList[i].spawnTileList)
                    {
                        int pathIndex = GetPathIndexInList(tiles.pathData.paths, worldTile);
                        if (pathIndex != -1)
                            spawnList[i].wave.Paths.Add(tiles.pathData.paths[pathIndex]);
                    }
                }
            }
        }

        private void SaveSpawnPointsAsPrefab(SpawnList[] spawnLists)
        {
            GameObject mainParent = new GameObject();
            foreach (var spawnList in spawnLists)
            {
                GameObject wave = new GameObject(spawnList.wave.name);
                wave.transform.SetParent(mainParent.transform);
                foreach (var spawnPoint in spawnList.spawnTileList)
                {
                    WorldTile spawn = (WorldTile)spawnPoint.Clone();
                    GameObject newPoint = new GameObject();
                    newPoint.AddComponent<WorldTile>();
                    spawnPoint.PasteThisComponentValues(ref newPoint);
                    Instantiate(newPoint.gameObject, wave.transform);
                }
            }

            PrefabUtility.SaveAsPrefabAsset(mainParent.gameObject, "Assets/Resources/spawnPoints.prefab");
        }

        [ExecuteInEditMode]
        private void ReadSavePointsPrefab(ref SpawnList[] spawnLists)
        {
            GameObject spawnList = Resources.Load<GameObject>("spawnPoints");
            if(spawnList != null)
            {
                for (int i = 0; i < spawnList.transform.childCount; i++)
                {
                    if (spawnList.transform.GetChild(i).childCount > 0)
                    {
                        spawnLists[i].spawnTileList.Clear();

                        for (int j = 0; j < spawnList.transform.GetChild(i).childCount; j++)
                            spawnLists[i].spawnTileList.Add(spawnList.transform.GetChild(i).GetChild(j).GetComponent<WorldTile>());
                    }
                }
            }
        }




        /// <summary>
        /// Get path index in list of path stores in PathDatas
        /// </summary>
        /// <param name="tileToCheck">Tile to check if path list contains tile</param>
        /// <param name="pathDataList">list of path in PathDatas</param>
        /// <returns>index of path or -1 if not found</returns>
        private int GetPathIndexInList(List<List<WorldTile>>pathDataList,WorldTile tileToCheck)
        {
            for (int i = 0; i < pathDataList.Count; i++)
            {
                if (pathDataList[i].Exists(x => x.GetComponent<WorldTile>().gridX == tileToCheck.gridX && x.GetComponent<WorldTile>().gridY == tileToCheck.gridY))
                    return i;
            }

            return -1;
        }

    }



    /// <summary>
    ///Interface for connection UI to WaveManager class
    /// </summary>
    public interface WaveInterface
    {
        void UIConnection(WaveManager waveManager);
    }

}
