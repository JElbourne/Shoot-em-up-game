using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInstance : MonoBehaviour {

    public Vector2 worldSize = new Vector2(8, 8); // Grid Size that holds individual rooms.
    public int roomTilesWide = 17;
    public int roomTilesHigh = 9;
    public int numberOfLevels = 1;

    int m_CurrentLevel = 1;

    [SerializeField]
	Texture2D[] m_RoomPatterns;

    Dictionary<int, GameObject> m_LevelsData = new Dictionary<int, GameObject>();
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
            m_LevelsData[i + 1] = m_LevelGo;

            AddRoomsToWorld(m_LevelGo, m_LevelGen.levelRooms, i+1);

            if (i != (m_CurrentLevel-1))
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

    public int GetCurrentLevel()
    {
        return m_CurrentLevel;
    }

    public int SetCurrentLevel(int _level)
    {
        m_CurrentLevel = _level;
        return m_CurrentLevel;
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
