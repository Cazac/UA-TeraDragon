using System;
using System.Collections.Generic;
using UnityEngine;

///////////////
/// <summary>
///     
/// WorldTile
/// 
/// </summary>
///////////////

// [RequireComponent(typeof(2D))]
public class WorldTile : MonoBehaviour, ICloneable
{

    public int gridY;
    public int gridX;


    //TO DO Remove ???
    public int posX;
    public int posY;

    public bool walkable;
    public bool towering;
    public bool isBlockedBarrier = false;
    public bool isDestroyed = false;


    public List<WorldTile> myNeighbours;

    public object Clone()
    {
        return MemberwiseClone();
    }

    public void PasteThisComponentValues(ref GameObject gameObjectToAddComp)
    {
        gameObjectToAddComp.GetComponent<WorldTile>().gridX = gridX;
        gameObjectToAddComp.GetComponent<WorldTile>().gridY = gridY;
        gameObjectToAddComp.GetComponent<WorldTile>().posX = posX;
        gameObjectToAddComp.GetComponent<WorldTile>().posY = posY;

        gameObjectToAddComp.GetComponent<WorldTile>().walkable= walkable;
        gameObjectToAddComp.GetComponent<WorldTile>().towering = towering;
        gameObjectToAddComp.GetComponent<WorldTile>().isBlockedBarrier = isBlockedBarrier;
        gameObjectToAddComp.GetComponent<WorldTile>().isDestroyed = isDestroyed;

    }



    /////////////////////////////////////////////////////////////////

    public override string ToString()
    {
        return "x: " + gridX.ToString() + " y: " + gridY.ToString();
    }
}
