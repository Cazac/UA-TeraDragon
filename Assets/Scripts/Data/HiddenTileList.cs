using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TransformList
{
    public Transform breakableBlockPos;
    public List<Transform> listOfNodes;

}

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
        this.list = new List<TransformList>(list);
    }
}
