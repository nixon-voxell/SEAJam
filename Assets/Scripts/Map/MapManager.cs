using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [Header("Developer")]
    [SerializeField] Tile _WallTile;
    [SerializeField] Tile _FloorTile;
    [SerializeField] Tile[] _ToReplaceWithWallTile;
    [SerializeField] bool _IsWallTileOnlyWalls;

    [Header("References")]
    [SerializeField] Camera _MapCamera;
    [SerializeField] Tilemap _WallTilemap;
    public Tilemap MapTilemap;
    [SerializeField] CanvasGroup _MapCanvas;
    [SerializeField] MapInteractable[] _MapInteractables;

    #region Properties

    MapInteractable _InteractedMap;

    #endregion

    private void Awake()
    {
        if(MapTilemap == null) MapTilemap = GetComponent<Tilemap>();
        AttachViewMap();
    }

    void AttachViewMap()
    {
        foreach(MapInteractable _map in _MapInteractables)
        {
            _map.AddInteraction(() => ViewMap(_map));
        }
    }

    public void ViewMap(MapInteractable mapPoint)
    {
        _InteractedMap = mapPoint;
        _MapCamera.enabled = true;
        Time.timeScale = 0;
        _MapCanvas.VisibleAndBlocks(true);
    }

    public void OnClickCloseMap()
    {
        _InteractedMap.CloseInteract();
        _InteractedMap = null;
        _MapCamera.enabled = false;
        Time.timeScale = 1;
        _MapCanvas.VisibleAndBlocks(false);
    }

    public void GenerateMap()
    {
        if(MapTilemap == null) MapTilemap = GetComponent<Tilemap>();

        BoundsInt _floorBounds = _WallTilemap.cellBounds;

        for (int x = 0; x < _floorBounds.size.x; x++)
        {
            for (int y = 0; y < _floorBounds.size.y; y++)
            {
                Vector3Int _cellPosition = new Vector3Int(x + _floorBounds.position.x, y + _floorBounds.position.y, 0);
                TileBase _currentTile = _WallTilemap.GetTile(_cellPosition);

                if (_currentTile == null) MapTilemap.SetTile(_cellPosition, _FloorTile);
                else
                {
                    if(_IsWallTileOnlyWalls) MapTilemap.SetTile(_cellPosition, _WallTile);
                    else if (ReplaceWithWall(_currentTile)) MapTilemap.SetTile(_cellPosition, _WallTile);
                }
            }
        }
    }
    public void ClearMap()
    {
        if (MapTilemap == null) MapTilemap = GetComponent<Tilemap>();
        MapTilemap.ClearAllTiles();
    }

    bool ReplaceWithWall(TileBase tile)
    {
        foreach (Tile _replaceTiles in _ToReplaceWithWallTile)
        {
            if (_replaceTiles != tile) continue;

            return true;
        }

        return false;
    }
}
