using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PathGenerator))]
public class PathEditor : Editor {

    public override void OnInspectorGUI()
    {
        PathGenerator pathGen = target as PathGenerator;
        
        if (DrawDefaultInspector())
        {
            pathGen.GeneratePath();
        }
        else if (GUILayout.Button("Generate"))
        {
            if (pathGen.useRandomSeed)
                pathGen.ChangeSeed();

            pathGen.GeneratePath();
            pathGen.ResetRooms();

            EditorUtility.SetDirty(pathGen.gameObject);
        }
        else if (GUILayout.Button("Reset Map"))
        {
            pathGen.ResetRooms();
        }

    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
