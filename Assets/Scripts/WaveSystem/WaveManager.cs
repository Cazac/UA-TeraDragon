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
        private bool enableSpawning;
        public bool EnableSpawning { get => enableSpawning; set => enableSpawning = value; }

        // public Vector3[] positions;
        public List<Vector3> NodeSpawnPosition { get => selectedNodeSpawnPosition; set => selectedNodeSpawnPosition = value; }

        // [SerializeField]
        // private float internalTimer;

        [Header("Scriptable wave object")]
        public Wave[] waves;

        private Wave currentWave;

        [SerializeField] //TODO: Delete serializeField
        private List<Vector3> selectedNodeSpawnPosition = new List<Vector3>();
        private const string WAVE_PARENT_NAME = "Parent_EnemyWave";

        private Timer currentTimer;
        private int waveIndex;

        public String TimeUntilNextWave
        {
            get
            {
                return currentTimer.TimeUntilNextSpawn.ToString();
            }
        }

        public String WaveTimer
        {
            get
            {
                return currentTimer.TimeUntilNextSpawn.ToString();
            }
        }


        private void Start()
        {
            MakeParent();
            StartCoroutine(SpawnSingleEnemyPerWave());
        }

        private void Update()
        {
            if (currentWave == null && EnableSpawning == true)
                currentWave = waves[0];

            if(currentTimer != null && EnableSpawning == true)
            {
                if(currentTimer.WaveCountdown(currentWave.WaveTimer))
                    EnableSpawning = false;
            }

            if(!EnableSpawning)
            {
                if(!currentTimer.NextWaveCountdown(currentWave.TimeUntilSpawn))
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
                            yield return new WaitForSeconds(currentWave.SpawnRatePerSecond);
                        }
                    }
                    EnableSpawning = false;

                    //Move on to the next scriptable wave in waves array
                    if(!AllWaveCompleted())
                    {
                        currentWave = waves[waveIndex++];
                        InstantiateNewTimer(currentWave.TimeUntilSpawn, currentWave.WaveTimer, ref currentTimer);
                    }
                }

                yield return null;
            }
        }

        private Boolean AllWaveCompleted()
        {
            if(waveIndex > waves.Length)
                return true;
            return false;
        }
    }

    public interface WaveInterface 
    {
        void UIConnection(WaveManager waveManager);
    }
}
