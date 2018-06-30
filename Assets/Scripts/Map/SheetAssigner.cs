using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetAssigner : MonoBehaviour {

    public int roomWidth = 17;
    public int roomHeight = 9;

    float m_TileSize = 1;
    Vector2 m_RoomDimensions;
    Vector2 m_GutterSize;

    [SerializeField]
	Texture2D[] sheetsNormal;
	[SerializeField]
	GameObject RoomObj;

    private void Awake()
    {
        m_RoomDimensions = new Vector2(m_TileSize * roomWidth, m_TileSize * roomHeight);
    }

    public void Assign(Room[,] rooms){
		foreach (Room room in rooms){
			//skip point where there is no room
			if (room == null){
				continue;
			}
			//pick a random index for the array
			int index = Mathf.RoundToInt(Random.value * (sheetsNormal.Length -1));
            //find position to place room
            Debug.Log(m_RoomDimensions);
			Vector3 pos = new Vector3(room.gridPos.x * (m_RoomDimensions.x), room.gridPos.y * (m_RoomDimensions.y), 0);
			RoomInstance myRoom = Instantiate(RoomObj, pos, Quaternion.identity, transform).GetComponent<RoomInstance>();
			myRoom.Setup(sheetsNormal[index], room.gridPos, room.type, room.doorTop, room.doorBot, room.doorLeft, room.doorRight);
		}
	}
}
