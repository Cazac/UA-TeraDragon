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
        if (GUILayout.Button("Build Object"))
        {
            myScript.EditorTestFunction();
        }
    }
}
