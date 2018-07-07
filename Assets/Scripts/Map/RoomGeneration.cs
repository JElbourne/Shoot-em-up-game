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
	GameObject doorU, doorD, doorL, doorR, doorWall;

	[SerializeField]
	ColorToGameObject[] mappings;

    [SerializeField]
    GameObject floorTile;

    [SerializeField]
    Color stairColor;

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
		Vector3 spawnPos = m_RoomRootTransform.position + Vector3.up*(roomSizeInTiles.y/2) - Vector3.up*(0.5f);
		PlaceDoor(spawnPos, doorTop, doorU);
		//bottom door
		spawnPos = m_RoomRootTransform.position + Vector3.down*(roomSizeInTiles.y/2) - Vector3.down*(0.5f);
		PlaceDoor(spawnPos, doorBot, doorD);
		//right door
		spawnPos = m_RoomRootTransform.position + Vector3.right*(roomSizeInTiles.x/2) - Vector3.right * (0.5f);
		PlaceDoor(spawnPos, doorRight, doorR);
		//left door
		spawnPos = m_RoomRootTransform.position + Vector3.left*(roomSizeInTiles.x/2) - Vector3.left * (0.5f);
		PlaceDoor(spawnPos, doorLeft, doorL);
	}
	void PlaceDoor(Vector3 _spawnPos, bool _door, GameObject _doorSpawn){
		// check whether its a door or wall, then spawn
		if (_door){
            m_WorldInstance.InstatiateWorldTile(_doorSpawn, _spawnPos, m_RoomRootTransform, level);
		}else{
            m_WorldInstance.InstatiateWorldTile(doorWall, _spawnPos, m_RoomRootTransform, level);
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
        

        if (pixelColor.a == 0){
            //clear spaces are the floor tiles
            m_WorldInstance.InstatiateWorldTile(floorTile, spawnPos, m_RoomRootTransform, level);
        } else if (stairColor.Equals(pixelColor) )
        {
            //find any potential stair locations
            m_WorldInstance.potentialStairLocations.Add(spawnPos);
        } else
        {
            //find the color to math the pixel
            foreach (ColorToGameObject mapping in mappings)
            {
                if (mapping.color.Equals(pixelColor))
                {
                    m_WorldInstance.InstatiateWorldTile(mapping.prefab, spawnPos, m_RoomRootTransform, level);
                }
            }
        }
	}
	Vector3 positionFromTileGrid(int x, int y){
		//find difference between the corner of the texture and the center of this object
		Vector3 offset = new Vector3(-(roomSizeInTiles.x/2) + 0.5f, -(roomSizeInTiles.y/2) + 0.5f, 0);
        //find scaled up position at the offset
        Vector3 pos = new Vector3(x,y,0) + offset + m_RoomRootTransform.position;
		return pos;
	}
}
