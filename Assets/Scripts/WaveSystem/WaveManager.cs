using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace WaveSystem
{
    public class WaveManager : MonoBehaviour
    {
        public float timeUntilSpawn;

        // public GameObject[] spawnEnemies; //TODO: Reimplement this!
        public GameObject spawnSingleEnemy;
        public bool enableSpawning;
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

        private Timer timer;
        private int waveIndex;

        private void Start()
        {
            MakeParent();
            StartCoroutine(SpawnSingleEnemyPerWave());
        }

        private void Update()
        {
            if (currentWave == null && enableSpawning == true)
            {
                currentWave = waves[0];
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

        public IEnumerator SpawnSingleEnemyPerWave()
        {
            while (true)
            {
                if (enableSpawning == true)
                {
                    currentWave.ParentGameobject = GameObject.Find(WAVE_PARENT_NAME);

                    foreach (var node in currentWave.SpawnPositionByNodes)
                    {
                        for (int i = 0; i < currentWave.NumberOfEnemyPerPos; i++)
                        {
                            Instantiate(currentWave.EnemyPrefab, node.transform.position, Quaternion.identity, currentWave.ParentGameobject.transform);
                            yield return new WaitForSeconds(timeUntilSpawn);
                        }
                    }
                    enableSpawning = false;

                    //Move on to the next scriptable wave in waves array
                    if(!AllWaveCompleted())
                    {
                        currentWave = waves[waveIndex++];
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
        void UIConnection();
    }
}
