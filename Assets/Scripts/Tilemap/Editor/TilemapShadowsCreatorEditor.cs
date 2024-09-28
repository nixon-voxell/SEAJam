using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(TilemapShadowsCreator))]
public class TilemapShadowsCreateEditor : Editor
{
    TilemapShadowsCreator script;

    private void OnEnable()
    {
        script = target as TilemapShadowsCreator;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Create Shadow Casters")) CreateShadowCasters();

        base.OnInspectorGUI();
    }

    void CreateShadowCasters()
    {
        Undo.RegisterFullObjectHierarchyUndo(script.transform, "Spawn Shadow Casters");
        Tilemap _connectedTilemap = script.GetComponent<Tilemap>();
        BoundsInt _mapBounds = _connectedTilemap.cellBounds;

        for (int x = 0; x < _mapBounds.size.x; x++)
        {
            for (int y = 0; y < _mapBounds.size.y; y++)
            {
                Vector3Int _tilePosition = new Vector3Int(x + _mapBounds.position.x, y + _mapBounds.position.y);
                Tile.ColliderType _collider = _connectedTilemap.GetColliderType(_tilePosition);

                switch (_collider)
                {
                    case Tile.ColliderType.None:
                        break;
                    default:
                        GameObject _newCaster = (GameObject)PrefabUtility.InstantiatePrefab(script._ShadowCaster, script.transform);
                        _newCaster.transform.position = _tilePosition + _connectedTilemap.tileAnchor; 
                        break;
                }

            }
        }
    }
}
