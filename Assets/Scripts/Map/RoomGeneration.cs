using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour {
	public Texture2D tex;
	[HideInInspector]
	public Vector2 gridPos;
	public int type; // 0: normal, 1: enter
    public int level;

	[HideInInspector]
	public bool doorTop, doorBot, doorLeft, doorRight;

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

	[SerializeField]
	ColorToGameObject[] mappings;

    [SerializeField]
    GameObject floorTile;

    [SerializeField]
    Color stairColor;

    [SerializeField]
    Color batteryColor;

    Vector2 roomSizeInTiles = new Vector2(17,9);

    WorldInstance m_WorldInstance;
    Transform m_RoomRootTransform;

    private void Awake()
    {
        m_WorldInstance = GetComponentInParent<WorldInstance>();
        roomSizeInTiles = new Vector2(m_WorldInstance.roomTilesWide, m_WorldInstance.roomTilesHigh);
    }

    public void SetupRoom(
            Transform _roomRootTransform,
            int _level,
            Texture2D _tex,
            Vector2 _gridPos,
            int _type,
            bool _doorTop,
            bool _doorBot,
            bool _doorLeft,
            bool _doorRight){
		tex = _tex;
        level = _level;
		gridPos = _gridPos;
		type = _type;
		doorTop = _doorTop;
		doorBot = _doorBot;
		doorLeft = _doorLeft;
		doorRight = _doorRight;
        m_RoomRootTransform = _roomRootTransform;

        MakeDoors();
		GenerateRoomTiles();
	}

	void MakeDoors(){
		//top door, get position then spawn
		Vector3 spawnPos1 = m_RoomRootTransform.position + (Vector3.up * (roomSizeInTiles.y / 2) - Vector3.up);
        Vector3 spawnPos2 = spawnPos1 - Vector3.right;
        PlaceDoor(spawnPos1, doorTop, doorU);
        PlaceDoor(spawnPos2, doorTop, doorU);
        //bottom door
        spawnPos1 = m_RoomRootTransform.position + (Vector3.down * (roomSizeInTiles.y / 2));
        spawnPos2 = spawnPos1 - Vector3.right;
        PlaceDoor(spawnPos1, doorBot, doorD);
        PlaceDoor(spawnPos2, doorBot, doorD);
        //right door
        spawnPos1 = m_RoomRootTransform.position + Vector3.right * (roomSizeInTiles.x / 2) - Vector3.right;
        spawnPos2 = spawnPos1 - Vector3.up;
		PlaceDoor(spawnPos1, doorRight, doorR);
        PlaceDoor(spawnPos2, doorRight, doorR);
        //left door
        spawnPos1 = m_RoomRootTransform.position + Vector3.left * (roomSizeInTiles.x / 2);
        spawnPos2 = spawnPos1 - Vector3.up;
		PlaceDoor(spawnPos1, doorLeft, doorL);
        PlaceDoor(spawnPos2, doorLeft, doorL);
    }
	void PlaceDoor(Vector3 _spawnPos, bool _door, GameObject _doorSpawn){
		// check whether its a door or wall, then spawn
		if (_door){
            m_WorldInstance.InstantiateMapTile(_doorSpawn, _spawnPos, m_RoomRootTransform, level);
		}else{
            m_WorldInstance.InstantiateMapTile(doorWall, _spawnPos, m_RoomRootTransform, level);
		}
	}

    void GenerateRoomTiles(){
		//loop through every pixel of the texture
		for(int x = 0; x < tex.width; x++){
			for (int y = 0; y < tex.height; y++){
				GenerateTile(x,y);
			}
		}
	}

	void GenerateTile(int x, int y){
		Color pixelColor = tex.GetPixel(x,y);
        Vector3 spawnPos = positionFromTileGrid(x, y);



        if (pixelColor.a == 0)
        {
            //clear spaces are the floor tiles
            m_WorldInstance.InstantiateMapTile(floorTile, spawnPos, m_RoomRootTransform, level);
        }
        else if (batteryColor.Equals(pixelColor))
        {
            // First we will set down the floor tile at the correct coord.
            m_WorldInstance.InstantiateMapTile(floorTile, spawnPos, m_RoomRootTransform, level);

            // Now register the battery with the world to instantiate based on world criteria
            spawnPos.z = level;
            m_WorldInstance.potentialBatteryLocations.Add(spawnPos);
            
        }
        else if (stairColor.Equals(pixelColor))
        {
            //find any potential stair locations
            spawnPos.z = level;
            m_WorldInstance.potentialStairLocations.Add(spawnPos);
        }
        else
        {
            //find the color to math the pixel
            foreach (ColorToGameObject mapping in mappings)
            {
                if (mapping.color.Equals(pixelColor))
                {
                    m_WorldInstance.InstantiateMapTile(mapping.prefab, spawnPos, m_RoomRootTransform, level);
                }
            }
        }
	}
	Vector3 positionFromTileGrid(int x, int y){
		//find difference between the corner of the texture and the center of this object
		Vector3 offset = new Vector3(-(roomSizeInTiles.x/2), -(roomSizeInTiles.y/2), 0);
        //find scaled up position at the offset
        Vector3 pos = new Vector3(x,y,0) + offset + m_RoomRootTransform.position;
		return pos;
	}
}
