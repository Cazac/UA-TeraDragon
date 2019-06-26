using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileNodes))]
public class TileNodesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TileNodes myScript = (TileNodes) target;
        if (GUILayout.Button("Build Map"))
        {
            myScript.BuildTable();
            
        }
        if (GUILayout.Button("Get List"))
        {
            myScript.Editor_SelectList();
        }

        if (GUILayout.Button("Test hide tile"))
        {
            myScript.HideTiles(myScript.hiddenTileManager.list, myScript.uniqueTilemap);
            //foreach (var item in myScript.hiddenTileManager.list)
            //{
            //    foreach (var item1 in item.list)
            //    {
            //        Debug.Log(item1.ToString());
            //    }
            //}
        }

        if (GUILayout.Button("Test show tile"))
        {
            myScript.ShowTiles(myScript.hiddenTileManager.list, myScript.uniqueTilemap);
            //foreach (var item in myScript.hiddenTileManager.list)
            //{
            //    foreach (var item1 in item.list)
            //    {
            //        Debug.Log(item1.ToString());
            //    }
            //}
        }

    }
}
