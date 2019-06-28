using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerScript : MonoBehaviour
{
    public int level;
    public CrystalTile crystalTile;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CrystalTimer();
    }


    void CrystalTimer()
    {
        timer += level * Time.fixedDeltaTime;
        if(timer > 100)
        {
            timer = 0;
            FindObjectOfType<PlayerStats>().AddCrystal(GetCrytals());
        }
    }

    public CrystalColor GetCrytals()
    {
        return crystalTile.ProduceRandomCrystal();
    }
}
