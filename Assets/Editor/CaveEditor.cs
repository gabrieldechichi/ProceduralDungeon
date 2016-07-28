using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CaveGenerator))]
public class CaveEditor : Editor
{

    public override void OnInspectorGUI()
    {
        CaveGenerator room = target as CaveGenerator;
        if (DrawDefaultInspector())
            room.GenerateMap();
    }
}
