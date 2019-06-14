using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewMapBound", menuName = "Scriptable Objects/Map Bound")]
public class MapBound : ScriptableObject
{
    //Where the map bounds will be placed next
    public int[] positionX;
}
