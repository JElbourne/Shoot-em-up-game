using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInstance : MonoBehaviour {

    public Vector2 worldSize = new Vector2(8, 8); // Grid Size that holds individual rooms.
    public int roomTilesWide = 17;
    public int roomTilesHigh = 9;
    public int numberOfLevels = 1;

    [HideInInspector]
    public List<Vector3> potentialStairLocations = new List<Vector3>();

    [HideInInspector]
    public int currentLevel = 1;

    [SerializeField]
	Texture2D[] m_RoomPatterns;

    public GameObject stairTile;
    public GameObject exitTile;

    [HideInInspector]
    public Dictionary<int, GameObject> levelsData = new Dictionary<int, GameObject>();
    
    Dictionary<Vector3, Tile> m_MapTileData = new Dictionary<Vector3, Tile>();

    public void Setup()
    {
        for(int i = 0; i < numberOfLevels; i++)
        {
            GameObject m_LevelGo = new GameObject("LevelRoot_" + (i + 1));
            m_LevelGo.transform.parent = transform;
            GameObject m_MiniMapGo = new GameObject("MiniMapRoot");
            m_MiniMapGo.transform.parent = m_LevelGo.transform;

            LevelGeneration m_LevelGen = GetComponent<LevelGeneration>();
            m_LevelGen.Setup(worldSize, i + 1, m_MiniMapGo.transform);
            levelsData[i + 1] = m_LevelGo;

            AddRoomsToWorld(m_LevelGo, m_LevelGen.levelRooms, i+1);

            bool exit = (i == numberOfLevels - 1);
            SelectAndPlaceStair(i + 1, m_LevelGo.transform, exit);
            

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
			int index = Mathf.RoundToInt(Random.value * (m_RoomPatterns.Length -1));
            //find position to place room
			Vector3 pos = new Vector3(room.gridPos.x * roomTilesWide, room.gridPos.y * roomTilesHigh, 0);
            GameObject m_RoomRoot = new GameObject("RoomRoot");
            m_RoomRoot.transform.position = pos;
            m_RoomRoot.transform.parent = _levelRoot.transform;

            // Debug.Log("Number of Room Patterns: " + m_RoomPatterns.Length + ", Current Index: " + index);
            GetComponent<RoomGeneration>().SetupRoom(
                m_RoomRoot.transform,
                _level,
                m_RoomPatterns[index],
                room.gridPos,
                room.type,
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

        InstatiateWorldTile(tile, currentPos, _parent, _level);
    }

    public void InstatiateWorldTile(GameObject _tileObject, Vector3 _spawnPos, Transform _transform, int _level)
    {
        GameObject m_TileGo = Instantiate(_tileObject, _spawnPos, Quaternion.identity);
        m_TileGo.transform.parent = _transform;
        SpriteRenderer m_SpriteRenderer = m_TileGo.GetComponent<SpriteRenderer>();
        // m_SpriteRenderer.enabled = false;
        m_SpriteRenderer.color = new Color(1f, 1f, 1f, 0f);

        Vector3 tileKey = new Vector3(_spawnPos.x, _spawnPos.y, _level);
        bool m_TileHasCollider = (m_TileGo.GetComponent<Collider2D>() != null);
        m_MapTileData[tileKey] = new Tile(m_TileGo, m_SpriteRenderer, _tileObject.name, m_TileHasCollider);
    }

    public bool isCoordInMapTileData(Vector3 coord)
    {
        return m_MapTileData.ContainsKey(coord);
    }

    public Tile getMapTileData(Vector3 coord)
    {
        if (m_MapTileData.ContainsKey(coord))
        {
            return m_MapTileData[coord];
        }
        return null;
    }

    public void LightWorld()
    {
        foreach(Tile tile in m_MapTileData.Values)
        {
            tile.SetOpacity(100f);
        }
    }
}
