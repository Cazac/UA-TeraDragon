using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile : MonoBehaviour
{

    public int gvalue;
    public int hValue;

    public int gridY;
    public int gridX;

    public bool walkable;

    public List<WorldTile> myNeighbours;
}
