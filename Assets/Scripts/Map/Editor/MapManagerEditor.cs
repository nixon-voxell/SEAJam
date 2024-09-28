using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapManager))]
public class MapManagerEditor : Editor
{
    MapManager MapGenerator;

    private void OnEnable()
    {
        MapGenerator = target as MapManager;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Generate Map"))
        {
            Undo.RecordObject(MapGenerator.MapTilemap, "Generated Wall Map");
            MapGenerator.GenerateMap();
        }

        if (GUILayout.Button("Clear Map"))
        {
            Undo.RecordObject(MapGenerator.MapTilemap, "Cleared Wall Map");
            MapGenerator.ClearMap();
        }

        base.OnInspectorGUI();
    }
}
