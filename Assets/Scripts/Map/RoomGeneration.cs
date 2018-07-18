using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour {
	RoomType m_RoomType;
    int m_Level;
	bool m_DoorTop, m_DoorBot, m_DoorLeft, m_DoorRight;
    Vector2 m_RoomSizeInTiles;
    WorldInstance m_WorldInstance;
    Transform m_RoomRootTransform;

    [SerializeField]
    GameObject doorU;
    [SerializeField]
    GameObject doorD;
    [SerializeField]
    GameObject doorL;
    [SerializeField]
    GameObject doorR;
    [SerializeField]
    GameObject doorWall;

    List<Vector3> m_AvailableFloorCoords = new List<Vector3>();
    List<Vector3> m_AvailableWallCoords = new List<Vector3>();
    List<Vector3> m_DoorCoords = new List<Vector3>();

    private void Awake()
    {
        m_WorldInstance = WorldInstance.instance;
        m_RoomSizeInTiles = new Vector2(m_WorldInstance.roomTilesWide, m_WorldInstance.roomTilesHigh);
    }

    public void SetupRoom(
            Transform _roomRootTransform,
            int _level,
            RoomType _roomType,
            bool _doorTop,
            bool _doorBot,
            bool _doorLeft,
            bool _doorRight){
        // Set variable
        m_RoomType = _roomType;
        m_Level = _level;
		m_DoorTop = _doorTop;
		m_DoorBot = _doorBot;
		m_DoorLeft = _doorLeft;
		m_DoorRight = _doorRight;
        m_RoomRootTransform = _roomRootTransform;

        // Create Rooms
        MakeDoors();
        GenerateRoomTiles();
        PickPotentialStairLocation();
        PickBatterySpawnerLocation();
    }

	void MakeDoors(){
		//top door, get position then spawn
		Vector3 spawnPos1 = m_RoomRootTransform.position + (Vector3.up * (m_RoomSizeInTiles.y / 2) - Vector3.up);
        Vector3 spawnPos2 = spawnPos1 - Vector3.right;
        PlaceDoor(spawnPos1, m_DoorTop, doorU);
        PlaceDoor(spawnPos2, m_DoorTop, doorU);
        //bottom door
        spawnPos1 = m_RoomRootTransform.position + (Vector3.down * (m_RoomSizeInTiles.y / 2));
        spawnPos2 = spawnPos1 - Vector3.right;
        PlaceDoor(spawnPos1, m_DoorBot, doorD);
        PlaceDoor(spawnPos2, m_DoorBot, doorD);
        //right door
        spawnPos1 = m_RoomRootTransform.position + Vector3.right * (m_RoomSizeInTiles.x / 2) - Vector3.right;
        spawnPos2 = spawnPos1 - Vector3.up;
		PlaceDoor(spawnPos1, m_DoorRight, doorR);
        PlaceDoor(spawnPos2, m_DoorRight, doorR);
        //left door
        spawnPos1 = m_RoomRootTransform.position + Vector3.left * (m_RoomSizeInTiles.x / 2);
        spawnPos2 = spawnPos1 - Vector3.up;
		PlaceDoor(spawnPos1, m_DoorLeft, doorL);
        PlaceDoor(spawnPos2, m_DoorLeft, doorL);
    }

	void PlaceDoor(Vector3 _spawnPos, bool _door, GameObject _doorSpawn){
        // check whether its a door or wall, then spawn
        m_DoorCoords.Add(_spawnPos);

        if (_door){
            m_WorldInstance.InstantiateMapTile(_doorSpawn, _spawnPos, m_RoomRootTransform, m_Level);
		}else{
            m_WorldInstance.InstantiateMapTile(doorWall, _spawnPos, m_RoomRootTransform, m_Level);
		}
	}

    void PickPotentialStairLocation()
    {
        if (m_AvailableFloorCoords.Count == 0)
        {
            Debug.LogWarning("You must run, GenerateRoomTiles, before picking potential stair locations in RoomGeneration.");
            return;
        }

        Vector3 m_SpawnPos = new Vector3();
        while(true)
        {
            m_SpawnPos = m_AvailableFloorCoords[Random.Range(0, m_AvailableFloorCoords.Count)];
            if (NotOnDoorwayAxis(m_SpawnPos))
            {
                break;
            }
        }
        m_AvailableFloorCoords.Remove(m_SpawnPos);
        m_SpawnPos.z = m_Level;
        m_WorldInstance.potentialStairLocations.Add(m_SpawnPos);
    }

    void PickBatterySpawnerLocation()
    {
        if (m_AvailableWallCoords.Count == 0)
        {
            Debug.LogWarning("You must run, GenerateRoomTiles, before picking potential battery locations in RoomGeneration.");
            return;
        }

        if (m_RoomType.numberOfBatterySpawners < 1)
        {
            Debug.LogWarning("You do not have any Battery Spawners in this room type.");
            return;
        }

        for (int i=0; i < m_RoomType.numberOfBatterySpawners; i++)
        {
            Vector3 m_SpawnPos = Vector3.zero;
            int m_LoopCounter = 0;
            while (m_LoopCounter < m_AvailableWallCoords.Count)
            {
                m_SpawnPos = m_AvailableWallCoords[Random.Range(0, m_AvailableWallCoords.Count)];
                m_AvailableWallCoords.Remove(m_SpawnPos);
                if (NotOnDoorwayAxis(m_SpawnPos) && HasFloorAsNeighbour(m_SpawnPos))
                {
                    break;
                }
                m_SpawnPos = Vector3.zero;
                m_LoopCounter++;
            }

            if (m_SpawnPos == Vector3.zero)
            {
                Debug.LogWarning("Could not find a spawner location after the while loop.");
            } else
            {
                m_SpawnPos.z = m_Level;
                m_WorldInstance.batteryLocations.Add(m_SpawnPos);
            }

        }
    }

    bool NotOnDoorwayAxis(Vector3 _coord)
    {
        return (
            (_coord.x != 0) &&
            (_coord.x != -1) &&
            (_coord.y != 0) &&
            (_coord.y != -1)
            );
    }

    bool HasFloorAsNeighbour(Vector3 _coord)
    {
        bool floorAsNeighbour = false;
        List<int> offsets = new List<int> { -1, 0, 1 };
        foreach (int i in offsets) {
            foreach (int j in offsets)
            {
                if (i == 0 && j == 0) // This would be the tile being tested
                    continue;

                if (Mathf.Abs(i) == 1 && Mathf.Abs(j) == 1) // This takes care of corner tiles
                    continue;

                Vector3 m_TestCoord = new Vector3(_coord.x + i, _coord.y + j, _coord.z);
                if (m_AvailableFloorCoords.Contains(m_TestCoord))
                {
                    return true;
                }
            }
        }
        return floorAsNeighbour;
    }

    void GenerateRoomTiles(){
		//loop through every pixel of the texture
		for(int x = 0; x < m_RoomType.roomPattern.width; x++){
			for (int y = 0; y < m_RoomType.roomPattern.height; y++){
				GenerateTile(x,y);
			}
		}
	}

	void GenerateTile(int x, int y){
		Color m_PixelColor = m_RoomType.roomPattern.GetPixel(x,y);
        Vector3 m_SpawnPos = positionFromTileGrid(x, y);

        if (!m_DoorCoords.Contains(m_SpawnPos))
        {
            if (m_PixelColor.a == 0)
            {
                //clear spaces are the floor tiles
                m_AvailableFloorCoords.Add(m_SpawnPos);
                m_WorldInstance.InstantiateMapTile(m_RoomType.floorTile, m_SpawnPos, m_RoomRootTransform, m_Level);
            }
            else
            {
                m_AvailableWallCoords.Add(m_SpawnPos);
                m_WorldInstance.InstantiateMapTile(m_RoomType.wallTile, m_SpawnPos, m_RoomRootTransform, m_Level);
            }
        }

        //else if (batteryColor.Equals(m_PixelColor))
        //{
            // First we will set down the floor tile at the correct coord.
        //    m_WorldInstance.InstantiateMapTile(floorTile, m_SpawnPos, m_RoomRootTransform, m_Level);

            // Now register the battery with the world to instantiate based on world criteria
        //    m_SpawnPos.z = m_Level;
        //    m_WorldInstance.potentialBatteryLocations.Add(m_SpawnPos);
            
        //}
	}
	Vector3 positionFromTileGrid(int x, int y){
		//find difference between the corner of the texture and the center of this object
		Vector3 offset = new Vector3(-(m_RoomSizeInTiles.x/2), -(m_RoomSizeInTiles.y/2), 0);
        //find scaled up position at the offset
        Vector3 pos = new Vector3(x,y,0) + offset + m_RoomRootTransform.position;
		return pos;
	}
}
