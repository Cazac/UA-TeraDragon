using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using WaveSystem;

public class WaveSystemTimer : MonoBehaviour, WaveInterface
{
    private WaveManager waveManager;

    private void Start() 
    {
        waveManager = GameObject.FindObjectOfType<WaveManager>();
    }

    public void UIConnection(WaveManager waveManager)
    {
        if(waveManager.EnableSpawning)
            this.GetComponent<Text>().text = "Wave " + waveManager.WaveIndex + " " + waveManager.WaveTimer;

        if(!waveManager.EnableSpawning)
            this.GetComponent<Text>().text = "Count down to next wave " + waveManager.TimeUntilNextWave;
    }

    private void Update() 
    {
        UIConnection(waveManager);
    }
}