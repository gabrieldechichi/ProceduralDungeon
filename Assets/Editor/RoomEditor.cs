using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(RoomGenerator))]
public class RoomEditor : Editor {

	public override void OnInspectorGUI()
    {
        RoomGenerator room = target as RoomGenerator;
        if (DrawDefaultInspector())
            room.GenerateMap();
    }
}
