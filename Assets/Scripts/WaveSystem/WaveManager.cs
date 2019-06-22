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
        public WaveData[] waves;


        [Header("DEBUG ONLY currentWave")]
        [SerializeField] 
        private WaveData currentWave;



        [SerializeField] //TODO: Delete serializeField
        private List<Vector3> selectedNodeSpawnPosition = new List<Vector3>();
        public List<Vector3> NodeSpawnPosition { get => selectedNodeSpawnPosition; set => selectedNodeSpawnPosition = value; }

        [Header("Parent Gameobject For Waves")]
        public GameObject waveParent;

        //private const string WAVE_PARENT_NAME = "Parent_EnemyWave";

        private Timer currentTimer;
        public WaveData CurrentWave { get => currentWave; set => currentWave = value; }
        private int waveIndex = 0;

        private TileNodes tileNodes;

        private CursorSelection cursorSelection;
        private SoundManager soundManager;
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
            soundManager = GameManager.FindObjectOfType<SoundManager>();
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
        
            CurrentWave = waves[0];
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

            //currentWave = waves[0];
            //MakeParent();

            //StartCoroutine(SpawnSingleEnemyPerWave());
        }

        private void Update()
        {
            //Create new timer object for current wave 
            if (EnableSpawning == true && currentTimer == null)
            {
                InstantiateNewTimer(CurrentWave.TimeUntilSpawn, CurrentWave.WaveTimer, ref currentTimer);
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

                //if(soundManager !=null)
                //{
                //    soundManager.PlaySpecificSound("Main");
                //    soundManager.ReturnControl = true;
                //}
            }

            //Debug.Log("Current wave timer: " + currentTimer.WaveTimer);
            //Debug.Log("Current wave timer interwave: " + currentTimer.TimeUntilNextSpawn);
        }


        /// <summary>
        ///Create new parent gameobject to store enemy info
        /// </summary>
        private void MakeParent()
        {
            //new GameObject(WAVE_PARENT_NAME);
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
            waveIndex = 0;
            //While loop to keep function running every frame if possible
            while (true)
            {
                //Debug.Log("Wave index: " + waveIndex);
                if (EnableSpawning == true)
                {
                    CurrentWave.ParentGameobject = waveParent;

                    foreach (List<WorldTile> path in CurrentWave.Paths)
                    {
                        for (int i = 0; i < CurrentWave.NumberOfEnemyPerPos; i++)
                        {
                            //Enumerator will return at this index, need to check if spawning option is still available
                            if (EnableSpawning == true)
                            {
                                GameObject enemy = Instantiate(CurrentWave.EnemyPrefab, path[0].transform.position, Quaternion.identity, CurrentWave.ParentGameobject.transform);
                                enemy.GetComponent<EnemyScript>().currentWaypoints = path;
                                yield return new WaitForSeconds(CurrentWave.SpawnRate);
                            }
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
                        CurrentWave = waves[++waveIndex];
                        //Debug.Log("Current wave: " + currentWave.ToString());

                        InstantiateNewTimer(CurrentWave.TimeUntilSpawn, CurrentWave.WaveTimer, ref currentTimer);

                        EnableSpawning = true;

                        //if (soundManager != null)
                        //{
                        //    soundManager.ReturnControl = false;
                        //    soundManager.PlaySpecificSound("Inter");
                        //}
                    }

                    else if (AllWaveCompleted())
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
            //Debug.Log("Wave index: " + waveIndex);

            if (waveIndex > waves.Length - 1)
            {
                //Debug.Log("End of all waves");
                return true;
            }
            return false;
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
