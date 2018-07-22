using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileGraph))]
public class WorldInstance : MonoBehaviour {

    public Vector2 worldSize = new Vector2(8, 8); // Grid Size that holds individual rooms.
    public int roomTilesWide = 34;
    public int roomTilesHigh = 18;
    public int numberOfLevels = 1;

    [HideInInspector]
    public List<Vector3> potentialStairLocations = new List<Vector3>();
    [HideInInspector]
    public List<Vector3> batteryLocations = new List<Vector3>();

    [HideInInspector]
    public int currentLevel = 1;

    [SerializeField]
    RoomType[] m_RoomTypes;

    public GameObject stairTile;
    public GameObject exitTile;
    public GameObject itemSpawnerTile;

    public Item batteryItem;

    [HideInInspector]
    public Dictionary<int, GameObject> levelsData = new Dictionary<int, GameObject>();

    [HideInInspector]
    public Dictionary<Vector3, Tile> mapTileData = new Dictionary<Vector3, Tile>();

    [HideInInspector]
    public Dictionary<Vector3, Tile> itemsData = new Dictionary<Vector3, Tile>(); // Non-Map Objects to light

    [HideInInspector]
    public TileGraph tileGraph;

    TileGraph m_LevelGrid;

    #region Singleton
    public static WorldInstance instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of World Instance!");
            return;
        }
        instance = this;

        m_LevelGrid = GetComponent<TileGraph>();
    }
    #endregion

    public void Setup()
    {
        for(int i = 0; i < numberOfLevels; i++)
        {
            GameObject m_LevelGo = new GameObject("LevelRoot_" + (i + 1));
            m_LevelGo.transform.parent = transform;
            GameObject m_MiniMapGo = new GameObject("MiniMapRoot_" + (i + 1));
            m_MiniMapGo.transform.parent = m_LevelGo.transform;

            LevelGeneration m_LevelGen = GetComponent<LevelGeneration>();
            m_LevelGen.Setup(worldSize, m_MiniMapGo.transform);
            levelsData[i + 1] = m_LevelGo;

            AddRoomsToWorld(m_LevelGo, m_LevelGen.levelRooms, i+1);

            bool exit = (i == numberOfLevels - 1);
            SelectAndPlaceStair(i + 1, m_LevelGo.transform, exit);
            SelectAndPlaceItemSpawner(batteryLocations, i + 1, m_LevelGo.transform, batteryItem);

            m_LevelGrid.Setup();

            if (i != (currentLevel-1))
            {
                m_LevelGo.SetActive(false);
            }


        }
    }

    void AddRoomsToWorld(GameObject _levelRoot, Room[,] _rooms, int _level){
		foreach (Room room in _rooms){
			//skip point where there is no room
			if (room == null){
				continue;
			}
			//pick a random index for the array
			int index = Mathf.RoundToInt(Random.value * (m_RoomTypes.Length -1));
            //find position to place room
			Vector3 pos = new Vector3(room.gridPos.x * roomTilesWide, room.gridPos.y * roomTilesHigh, 0);
            GameObject m_RoomRoot = new GameObject("RoomRoot");
            m_RoomRoot.transform.position = pos;
            m_RoomRoot.transform.parent = _levelRoot.transform;

            // Debug.Log("Number of Room Patterns: " + m_RoomPatterns.Length + ", Current Index: " + index);
            GetComponent<RoomGeneration>().SetupRoom(
                m_RoomRoot.transform,
                _level,
                m_RoomTypes[index],
                room.doorTop,
                room.doorBot,
                room.doorLeft,
                room.doorRight);
		}
	}

    void SelectAndPlaceStair(int _level, Transform _parent, bool _exit)
    {
        float largestDist = 0f;
        Vector3 currentPos = Vector3.zero;
        GameObject tile = (_exit) ? exitTile : stairTile;

        foreach (Vector3 stairCoord in potentialStairLocations)
        {
            if (stairCoord.z == _level) // make sure we only look at the stair locations for the current Level. 
            {
                Vector3 stairPos = new Vector3(stairCoord.x, stairCoord.y, 0f); // Reset the z coord back to 0. 
                float dist = Vector3.Distance(stairPos, Vector3.zero);
                if (dist > largestDist)
                {
                    largestDist = dist;
                    currentPos = stairPos;
                }
            }
        }

        InstantiateMapTile(tile, currentPos, _parent, _level);
    }

    void SelectAndPlaceItemSpawner(List<Vector3> _locations, int _level, Transform _parent, Item _item)
    {
        foreach (Vector3 m_Coord in _locations)
        {
            if (m_Coord.z == _level) // make sure we only look at the stair locations for the current Level. 
            {
                // Debug.Log("Placing Battery");
                Vector3 batteryPos = new Vector3(m_Coord.x, m_Coord.y, 0f); // Reset the z coord back to 0. 
                Tile m_Tile = ReplaceMapTile(itemSpawnerTile, batteryPos, _parent, _level);
                m_Tile.tileGo.GetComponent<ItemSpawnController>().item = _item;
            }
        }
    }

    public void InstantiateMapTile(GameObject _tileObject, Vector3 _spawnPos, Transform _parentTransform, int _level)
    {
        Tile m_Tile = InstantiateTile(_tileObject, _spawnPos, _parentTransform, _level);
        Vector3 tileKey = new Vector3(_spawnPos.x, _spawnPos.y, _level);
        mapTileData[tileKey] = m_Tile;
    }

    public Tile ReplaceMapTile(GameObject _tileObject, Vector3 _spawnPos, Transform _parentTransform, int _level)
    {
        Tile m_Tile = InstantiateTile(_tileObject, _spawnPos, _parentTransform, _level);
        Vector3 tileKey = new Vector3(_spawnPos.x, _spawnPos.y, _level);

        if(mapTileData.ContainsKey(tileKey))
        {
            Tile m_OriginalTile = mapTileData[tileKey];
            Destroy(m_OriginalTile.tileGo);
            //Debug.Log("Tile Destroyed for Replacment");
        }
        
        mapTileData[tileKey] = m_Tile;
        return m_Tile;
    }

    Tile InstantiateTile(GameObject _tileObject, Vector3 _spawnPos, Transform _parentTransform, int _level)
    {
        GameObject m_TileGo = Instantiate(_tileObject, _spawnPos, Quaternion.identity, _parentTransform);
        SpriteRenderer m_SpriteRenderer = m_TileGo.GetComponent<SpriteRenderer>();
        // m_SpriteRenderer.enabled = false;
        m_SpriteRenderer.color = new Color(1f, 1f, 1f, 0f);

        bool m_TileHasCollider = (m_TileGo.GetComponent<Collider2D>() != null);
        return new Tile(m_TileGo, m_SpriteRenderer, _tileObject.name, m_TileHasCollider);
    }

    public bool isCoordInMapTileData(Vector3 coord)
    {
        return mapTileData.ContainsKey(coord);
    }

    public int[] getTileCoord(Transform _transform)
    {
        int posX = Mathf.RoundToInt(_transform.position.x);
        int posY = Mathf.RoundToInt(_transform.position.y);
        return new int[2] { posX, posY };
    }

    public Vector2 getTilePosition(Vector3 _coord)
    {
        int posX = Mathf.RoundToInt(_coord.x);
        int posY = Mathf.RoundToInt(_coord.y);
        return new Vector2( posX, posY );
    }

    public Tile[] getTileNeighbours(Vector3 _tileCoord)
    {
        List<Tile> m_Neighbours = new List<Tile>();
        for (int x = -1; x < 1; x++)
        {
            for (int y = -1; y < 1; y++)
            {
                Tile m_Tile = getMapTileData(new Vector3(_tileCoord.x + x, _tileCoord.y + y, _tileCoord.z));
                m_Neighbours.Add(m_Tile);
            }
        }
        return m_Neighbours.ToArray();
    }

    public Tile getMapTileData(Vector3 coord)
    {
        if (mapTileData.ContainsKey(coord))
        {
            return mapTileData[coord];
        }
        return null;
    }

    public int GetTileMovementCost(Vector3 _coord)
    {
        if (mapTileData.ContainsKey(_coord))
        {
            return mapTileData[_coord].movementCost;
        }
        // If can not find a tile we will default to not a walkable tile so has 0 movement
        return 0;
    }

    public bool isTileCollider(Vector3 _coord)
    {
        if (mapTileData.ContainsKey(_coord))
        {
            return mapTileData[_coord].hasCollider();
        }
        // If can not find a tile we will default to not a walkable tile so has collider
        return true;
    }

    public Tile getItemsData(Vector3 _coord)
    {
        if (itemsData.ContainsKey(_coord))
        {
            return itemsData[_coord];
        }
        return null;
    }

    public void RemoveFromItemsData(Transform _transform)
    {
        Vector3 m_Coord = new Vector3(_transform.position.x, _transform.position.y, currentLevel);
        if (itemsData.ContainsKey(m_Coord))
        {
            itemsData.Remove(m_Coord);
        }
    } 

    public void LightWorld()
    {
        foreach(Tile tile in mapTileData.Values)
        {
            tile.SetOpacity(100f);
        }

        foreach (Tile tile in itemsData.Values)
        {
            tile.SetOpacity(100f);
        }
    }
}
