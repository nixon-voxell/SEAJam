using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapShadowsCreator : MonoBehaviour
{
    public GameObject _ShadowCaster;

    //[ContextMenu("Create Shadow Casters")]
    //void CreateShadowCasters()
    //{
    //    Tilemap _floorMap = GetComponent<Tilemap>();
    //    BoundsInt _mapBounds = _floorMap.cellBounds;

    //    for (int x = 0; x < _mapBounds.size.x; x++)
    //    {
    //        for (int y = 0; y < _mapBounds.size.y; y++)
    //        {
    //            Vector3Int _tilePosition = new Vector3Int(x + _mapBounds.position.x, y + _mapBounds.position.y);
    //            Tile.ColliderType _collider = _floorMap.GetColliderType(_tilePosition);

    //            switch(_collider)
    //            {
    //                case Tile.ColliderType.None:
    //                    break;
    //                default:
    //                    Instantiate(_ShadowCaster, _tilePosition + _floorMap.tileAnchor, Quaternion.identity, _floorMap.transform);
    //                    break;
    //            }

    //        }
    //    }
    //}
}
