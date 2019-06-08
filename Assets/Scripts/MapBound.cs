using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewMapBound", menuName = "Scriptable Objects/Map Bound")]
public class MapBound : ScriptableObject
{

    public int waveID;
    public int positionX;


    public List<TileBase> NewTiles;

}
