using UnityEngine;
using UnityEditor; // This is required for editor scripting

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapGenerator mapGenerator = (MapGenerator)target;

        if (GUILayout.Button("Generate Map"))
        {
            mapGenerator.GenerateMap();
        }
    }
}