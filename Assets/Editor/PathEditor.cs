using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PathGenerator))]
public class PathEditor : Editor {

    public override void OnInspectorGUI()
    {
        PathGenerator pathGen = target as PathGenerator;
        if (DrawDefaultInspector() || GUILayout.Button("Generate"))
        {
            if (pathGen.useRandomSeed)
                pathGen.ChangeSeed();

            pathGen.GeneratePath();
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
