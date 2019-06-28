using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalTile : WorldTile
{
    [Range(10, 50)]
    public int crystalValueRed = 25;
    [Range(10, 50)]
    public int crystalValueBlue = 25;
    [Range(10, 50)]
    public int crystalValueGreen = 25;
    [Range(10, 50)]
    public int crystalValueYellow = 25;

    private void Start()
    {
    //    RandomCrystalValues();
    }
    /// <summary>
    /// Selects a color for the crystal. probabilities are determined by each Crystal values
    /// </summary>
    /// <returns>Color of the crystal</returns>
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

    /// <summary>
    /// Generates random values for each crystal values
    /// </summary>
    void RandomCrystalValues()
    {
        crystalValueRed = Random.Range(10, 51);
        crystalValueBlue = Random.Range(10, 51);
        crystalValueGreen = Random.Range(10, 51);
        crystalValueYellow = Random.Range(10, 51);
    }

    /// <summary>
    /// Set crystal tile such a that only 1 color crystal is produced
    /// </summary>
    /// <param name="cc">Unique color chosen</param>
    void UniqueCrystalValue(CrystalColor cc)
    {
        switch (cc)
        {
            case CrystalColor.RED:
                crystalValueRed = 1;
                crystalValueBlue = crystalValueGreen = crystalValueYellow = 0;
                return;
            case CrystalColor.YELLOW:
                crystalValueYellow = 1;
                crystalValueBlue = crystalValueGreen = crystalValueRed = 0;
                return;
            case CrystalColor.BLUE:
                crystalValueBlue = 1;
                crystalValueRed = crystalValueGreen = crystalValueYellow = 0;
                return;
            case CrystalColor.GREEN:
                crystalValueGreen = 1;
                crystalValueBlue = crystalValueRed = crystalValueYellow = 0;
                return;
        }
    }
}

public enum CrystalColor {
    RED,
    YELLOW,
    BLUE,
    GREEN
}

