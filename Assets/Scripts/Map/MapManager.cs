using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

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

    #region Properties

    MapInteractable _InteractedMap;
    List<MapInteractable> _MapInteractables = new List<MapInteractable>();

    #endregion

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            this.enabled = false;
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        if(MapTilemap == null) MapTilemap = GetComponent<Tilemap>();
        SceneManager.sceneLoaded += ResetReferences;
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
            SceneManager.sceneLoaded -= ResetReferences;
        }
    }

    void ResetReferences(Scene loadedScene, LoadSceneMode loadMode)
    {
        _MapInteractables.Clear();
        _InteractedMap = null;
    }

    public void AttachMap(MapInteractable map)
    {
        _MapInteractables.Add(map);
        map.AddInteraction(() => ViewMap(map));
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
                Tile _currentTile = (Tile)_WallTilemap.GetTile(_cellPosition);

                if (_currentTile == null) continue;// MapTilemap.SetTile(_cellPosition, _FloorTile);
                else
                {
                    if (_IsWallTileOnlyWalls) MapTilemap.SetTile(_cellPosition, _WallTile);
                    else
                    {
                        switch (_currentTile.colliderType)
                        {
                            case Tile.ColliderType.None:
                                MapTilemap.SetTile(_cellPosition, _FloorTile);
                                break;
                            default:
                                MapTilemap.SetTile(_cellPosition, _WallTile);
                                break;
                        }
                    }
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
