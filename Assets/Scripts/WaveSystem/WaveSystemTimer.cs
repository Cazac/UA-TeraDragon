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
        {
            //TO DO Fix the last bit as a it casuses arrors due to invalid reff
            gameObject.GetComponent<Text>().text = "Wave " +  waveManager.WaveIndex + " " /*+  waveManager.WaveTimer */;
        }

        if(!waveManager.EnableSpawning)
        {
            gameObject.GetComponent<Text>().text = "Count down to next wave " + waveManager.TimeUntilNextWave;
        }
    }

    private void Update() 
    {
        if (waveManager != null)
        {
            UIConnection(waveManager);
        }
    }
}
