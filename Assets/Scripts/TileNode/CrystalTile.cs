using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalTile : WorldTile
{

    public int crystalValueRed = 25;
    public int crystalValueBlue = 25;
    public int crystalValueGreen = 25;
    public int crystalValueYellow = 25;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public CrystalColor ProduceRandomCrystal()
    {
        int total = crystalValueRed + crystalValueBlue + crystalValueGreen + crystalValueYellow;
        int randVale = Random.Range(0, total);

        if (randVale < crystalValueRed)
        {
            return CrystalColor.RED;
        }
        else if (randVale < crystalValueRed + crystalValueBlue)
        {
            return CrystalColor.BLUE;
        }
        else if (randVale < crystalValueRed + crystalValueBlue + crystalValueGreen)
        {
            return CrystalColor.GREEN;
        }
        else
        {
            return CrystalColor.YELLOW;
        }
    }

    void RandomCrystalValues()
    {
        crystalValueRed = Random.Range(10, 51);
        crystalValueBlue = Random.Range(10, 51);
        crystalValueGreen = Random.Range(10, 51);
        crystalValueYellow = Random.Range(10, 51);
    }
}

public enum CrystalColor {
    RED,
    YELLOW,
    BLUE,
    GREEN
}

