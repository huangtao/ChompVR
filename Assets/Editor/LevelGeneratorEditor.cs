using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelGenerator script = (LevelGenerator)target;

        if (GUILayout.Button("Generate Level"))
        {
            script.GenerateLevel();
        }
    }
}