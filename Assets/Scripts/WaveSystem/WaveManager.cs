using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace WaveSystem
{
    public class WaveManager : MonoBehaviour
    {
        // public GameObject[] spawnEnemies; //TODO: Reimplement this!
        public GameObject spawnSingleEnemy;
        public bool enableSpawning;
        public bool EnableSpawning { get => enableSpawning; set => enableSpawning = value; }

        // public Vector3[] positions;
        public List<Vector3> NodeSpawnPosition { get => selectedNodeSpawnPosition; set => selectedNodeSpawnPosition = value; }

        // [SerializeField]
        // private float internalTimer;

        [Header("Scriptable wave object")]
        public Wave[] waves;

        public Wave currentWave;

        [SerializeField] //TODO: Delete serializeField
        private List<Vector3> selectedNodeSpawnPosition = new List<Vector3>();
        private const string WAVE_PARENT_NAME = "Parent_EnemyWave";

        private Timer currentTimer;
        private int waveIndex = 0;

        public int WaveIndex
        {
            get => waveIndex;
        }

        public String TimeUntilNextWave
        {
            get
            {
                if(currentTimer != null)
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
            currentWave = waves[0];
        }


        private void Start()
        {
            MakeParent();

            StartCoroutine(SpawnSingleEnemyPerWave());
        }

        private void Update()
        {
            if (EnableSpawning == true && currentTimer == null)
            {
                InstantiateNewTimer(currentWave.TimeUntilSpawn, currentWave.WaveTimer, ref currentTimer);

            }

            if(currentTimer != null && EnableSpawning == true)
            {
                if(currentTimer.WaveCountdown())
                    EnableSpawning = false;
            }

            if(currentTimer != null && EnableSpawning == false && !AllWaveCompleted())
            {
                if(currentTimer.NextWaveCountdown())
                    EnableSpawning = true;
            }
        }


        /// <summary>
        ///Create new parent gameobject to store enemy info
        /// </summary>
        private void MakeParent()
        {
            Instantiate(new GameObject(WAVE_PARENT_NAME));
        }

        private void InstantiateNewTimer(float timeUntilSpawn, float waveTimer, ref Timer timer)
        {
            timer = null;
            timer = new Timer(timeUntilSpawn, waveTimer);
        }

        public void SpawnEnemyWorldPos(GameObject enemyPrefab, Vector3 spawnPosition, GameObject parentGameobject)
        {
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, parentGameobject.transform);
        }

        public IEnumerator SpawnSingleEnemyPerWave()
        {
            while (true)
            {
                if (EnableSpawning == true )
                {
                    currentWave.ParentGameobject = GameObject.Find(WAVE_PARENT_NAME);

                    foreach (var node in currentWave.SpawnPositionByNodes)
                    {
                        for (int i = 0; i < currentWave.NumberOfEnemyPerPos; i++)
                        {
                            Instantiate(currentWave.EnemyPrefab, node.transform.position, Quaternion.identity, currentWave.ParentGameobject.transform);

                            //Enumerator will return at this index, need to check if spawning option is still available
                            if(EnableSpawning)
                            {
                                yield return new WaitForSeconds(currentWave.SpawnRatePerSecond);
                            }
                        }
                    }

                    EnableSpawning = false;


                    Debug.Log("Check current wave");
                    //Move on to the next scriptable wave in waves array
                    if(!AllWaveCompleted() && currentTimer.WaveTimer <= 0)
                    {
                        Debug.Log("Check current wave inside");
                        currentWave = null;
                        currentWave = waves[++waveIndex];
                        Debug.Log("Current wave: " + currentWave.ToString());

                        InstantiateNewTimer(currentWave.TimeUntilSpawn, currentWave.WaveTimer, ref currentTimer);
                    }

                    if(AllWaveCompleted())
                    {
                        EnableSpawning = false;
                        StopCoroutine(SpawnSingleEnemyPerWave());
                    }
                }

                yield return null;
            }
        }

        private Boolean AllWaveCompleted()
        {
            Debug.Log("Wave index: " + waveIndex);

            if(waveIndex > waves.Length - 1)
            {
                Debug.Log("End of alll waves");
                return true;
            }
            return false;
        }
    }

    public interface WaveInterface 
    {
        void UIConnection(WaveManager waveManager);
    }
}
