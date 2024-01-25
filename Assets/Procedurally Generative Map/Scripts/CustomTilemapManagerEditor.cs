using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
// Editor-specific code here

[CustomEditor(typeof(CustomTilemapManager))]
public class CustomTilemapManagerEditor : Editor
{
    private enum Tool
    {
        Cursor,
        Brush,
        Eraser
    }

    private Tool selectedTool = Tool.Cursor;

    void OnEnable()
    {
        SceneView.duringSceneGui += DuringSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Cursor"))
        {
            selectedTool = Tool.Cursor;
        }

        if (GUILayout.Button("Brush"))
        {
            selectedTool = Tool.Brush;
            Debug.Log("Placing...");
        }

        if (GUILayout.Button("Eraser"))
        {
            selectedTool = Tool.Eraser;
        }
    }

    private void DuringSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Debug.Log("Clicked!");
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;

            // Optional: Define a raycast distance
            float raycastDistance = 100f; // Example distance

            // Visualize the raycast (for debugging)
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * raycastDistance, Color.red, 1f);

            if (Physics.Raycast(ray, out hit, raycastDistance)) // Include distance
            {
                Vector3Int position = GetTilePositionFromHit(hit);
                Debug.Log("Click position: " + position);
                CustomTilemapManager mapManager = (CustomTilemapManager)target;

                switch (selectedTool)
                {
                    case Tool.Brush:
                        mapManager.PlaceTile(position.x, position.y, 0);
                        break;
                    case Tool.Eraser:
                        mapManager.RemoveTile(position.x, position.y);
                        break;
                }
            }
        }
    }

    private Vector3Int GetTilePositionFromHit(RaycastHit hit)
    {
        // Convert world position to tilemap position
        // This may need adjustments depending on your tile size and pivot
        Vector3 hitPosition = hit.point;
        int x = Mathf.FloorToInt(hitPosition.x);
        int y = Mathf.FloorToInt(hitPosition.y);
        return new Vector3Int(x, y, 0);
    }
}

#endif