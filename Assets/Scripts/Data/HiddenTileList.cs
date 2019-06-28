using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Serilizable class to store breakableBlock and list of nodes that connects to breakableBlock
/// </summary>
[System.Serializable]
public class TransformList
{
    public Transform breakableBlockPos;
    public List<Transform> listOfNodes;

    public TransformList(Transform breakableBlockPos, List<Transform> listOfNodes)
    {
        this.breakableBlockPos = breakableBlockPos;
        this.listOfNodes = listOfNodes;
    }
}

/// <summary>
/// Serilizable class to store list of TransformList object
/// </summary>
[System.Serializable]
public class HiddenTileManager
{
    public List<TransformList> list;

    public HiddenTileManager()
    {
        list = new List<TransformList>();
    }

    public HiddenTileManager(List<TransformList> list)
    {
        this.list = new List<TransformList>();
        foreach (TransformList item in list)
        {
            this.list.Add(new TransformList(item.breakableBlockPos, item.listOfNodes));
        }
    }
}
