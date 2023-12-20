using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

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

#endif