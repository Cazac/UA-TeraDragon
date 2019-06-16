using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////
/// <summary>
///     
/// WorldTile
/// 
/// </summary>
///////////////

[RequireComponent(typeof(SphereCollider))]
public class WorldTile : MonoBehaviour
{
    public int gridY;
    public int gridX;


    //TO DO Remove ???
    public int posX;
    public int posY;

    public bool walkable;
    public bool towering;

    public List<WorldTile> myNeighbours;

    /////////////////////////////////////////////////////////////////

    public override string ToString()
    {
        return "x: " + gridX.ToString() + " y: " + gridY.ToString();
    }
}
