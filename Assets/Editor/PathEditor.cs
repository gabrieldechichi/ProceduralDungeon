using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PathGenerator))]
public class PathEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PathGenerator pathGen = target as PathGenerator;
        if (GUILayout.Button("Generate"))
        {
            pathGen.Draw();
            EditorUtility.SetDirty(pathGen.gameObject);
        }

    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
