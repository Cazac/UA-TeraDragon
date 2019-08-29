using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using WaveSystem;

[CustomEditor(typeof(WaveManager))]
public class SpawnEditor : Editor
{
    public Vector3 test;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Button("Test");
    }
}
