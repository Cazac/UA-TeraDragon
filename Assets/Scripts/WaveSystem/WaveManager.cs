using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace WaveSystem
{
    public class WaveManager : MonoBehaviour
    {
        public TileNodes tiles;

        // public GameObject[] spawnEnemies; //TODO: Reimplement this!
        public GameObject spawnSingleEnemy;
        public bool enableSpawning;
        public bool EnableSpawning { get => enableSpawning; set => enableSpawning = value; }

        // public Vector3[] positions;

        // [SerializeField]
        // private float internalTimer;

        [Header("Scriptable wave object")]
        public Wave[] waves;

        public Wave currentWave;

        [SerializeField] //TODO: Delete serializeField
        private List<Vector3> selectedNodeSpawnPosition = new List<Vector3>();
        public List<Vector3> NodeSpawnPosition { get => selectedNodeSpawnPosition; set => selectedNodeSpawnPosition = value; }

        private const string WAVE_PARENT_NAME = "Parent_EnemyWave";

        private Timer currentTimer;
        private int waveIndex = 0;

        private TileNodes tileNodes;

        private CursorSelection cursorSelection;
        public int WaveIndex
        {
            get => waveIndex;
        }

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


        private void Awake()
        {
        }


        private void Start()
        {
            tileNodes = GameObject.FindObjectOfType<TileNodes>();
            //Cached CursorSelection
            cursorSelection = GameObject.FindObjectOfType<CursorSelection>();

            for (int i = 0; i < waves.Length; i++)
            {
                if (i >= tiles.pathData.paths.Count)
                {
                    waves[i].Paths = new List<List<WorldTile>>()
                    {
                        tiles.pathData.paths[tiles.pathData.paths.Count-1]
                    };
                }
                else
                {
                    waves[i].Paths = new List<List<WorldTile>>() { tiles.pathData.paths[i] };
                }
            }

            //for(int i = 0; i < tiles.pathData.paths.Count; i++)
            //{
               // waves[i].Paths = new List<List<WorldTile>>()
                   // {
                      //  tiles.pathData.paths[i]
                   // };
            //}
        
            currentWave = waves[0];
            MakeParent();

            StartCoroutine(SpawnSingleEnemyPerWave());
            // // for testing purposes
            // // gives waves paths form shortest to longest
            //for (int i = 0; i < waves.Length; i++)
            //{
            //    if (i >= tiles.pathData.paths.Count)
            //    {
            //     //   waves[i].Paths = new List<List<WorldTile>>() { tiles.pathData.paths[tiles.pathData.paths.Count-1] };
            //    }
            //    else
            //    {
            //     //   waves[i].Paths = new List<List<WorldTile>>() { tiles.pathData.paths[i] };
            //    }
            //}

            currentWave = waves[0];
            MakeParent();

            StartCoroutine(SpawnSingleEnemyPerWave());
        }

        private void Update()
        {
            //Create new timer object for current wave 
            if (EnableSpawning == true && currentTimer == null)
            {
                InstantiateNewTimer(currentWave.TimeUntilSpawn, currentWave.WaveTimer, ref currentTimer);
            }

            //If timer for a wave hits 0, turn off spawning
            if (currentTimer != null && EnableSpawning == true)
            {
                if (currentTimer.WaveCountdown())
                    EnableSpawning = false;
            }


            //If timer between wave hits 0, turn on spawning
            if (currentTimer != null && EnableSpawning == false && !AllWaveCompleted())
            {
                if (currentTimer.NextWaveCountdown())
                    EnableSpawning = true;
            }
        }


        /// <summary>
        ///Create new parent gameobject to store enemy info
        /// </summary>
        private void MakeParent()
        {
            new GameObject(WAVE_PARENT_NAME);
        }


        /// <summary>
        ///Create a new timer instance when a wave is finished
        /// </summary>
        /// <param name="timeUntilSpawn">Timer between wave until next wave starts</param>
        /// <param name="waveTimer">Timer for current wave</param>
        /// <param name="timer">ref variable timer to reference the timer instance</param>
        private void InstantiateNewTimer(float timeUntilSpawn, float waveTimer, ref Timer timer)
        {
            timer = null;
            timer = new Timer(timeUntilSpawn, waveTimer);
        }

        /// <summary>
        ///Main method for spawning a single type of enemy in multiple position
        /// </summary>
        /// <remarks>
        ///Use IEnumerator and must only be called in Start() 
        ///</remarks>
        public IEnumerator SpawnSingleEnemyPerWave()
        {
            //While loop to keep function running every frame if possible
            while (true)
            {
                Debug.Log("Wave index: " + waveIndex);
                if (EnableSpawning == true)
                {
                    currentWave.ParentGameobject = GameObject.Find(WAVE_PARENT_NAME);

                    foreach (List<WorldTile> path in currentWave.Paths)
                    {
                        for (int i = 0; i < currentWave.NumberOfEnemyPerPos; i++)
                        {
                            GameObject enemy = Instantiate(currentWave.EnemyPrefab, path[0].transform.position, Quaternion.identity, currentWave.ParentGameobject.transform);
                            enemy.GetComponent<EnemyScript>().waypoints = path;
                            //Enumerator will return at this index, need to check if spawning option is still available
                            if (EnableSpawning)
                            {
                                yield return new WaitForSeconds(currentWave.SpawnRate);
                            }
                        }
                    }

                    EnableSpawning = false;


                    //print(currentTimer.WaveTimer);

                    //Move on to the next scriptable wave in waves array
                    if (!AllWaveCompleted() && currentTimer.WaveTimer <= 0)
                    {
                        currentWave = null;
                        currentWave = waves[++waveIndex];
                        Debug.Log("Current wave: " + currentWave.ToString());

                        InstantiateNewTimer(currentWave.TimeUntilSpawn, currentWave.WaveTimer, ref currentTimer);
                    }

                    if (AllWaveCompleted())
                    {
                        EnableSpawning = false;
                        StopCoroutine(SpawnSingleEnemyPerWave());
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
            Debug.Log("Wave index: " + waveIndex);

            if (waveIndex > waves.Length - 1)
            {
                Debug.Log("End of alll waves");
                return true;
            }
            return false;
        }

        public void WaveStartPosSelection()
        {
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
