using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CorridorGenerator))]
public class CorridorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        CorridorGenerator corridor = target as CorridorGenerator;
        if (DrawDefaultInspector())
            corridor.GenerateMap();
    }
}

