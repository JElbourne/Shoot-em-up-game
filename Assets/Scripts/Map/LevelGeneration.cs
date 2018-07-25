using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {
	
	List<Vector2> m_TakenPositions = new List<Vector2>();
	int m_GridSizeX, m_GridSizeY, m_NumberOfRooms = 20;
    Transform m_MiniMapTransform;

    public Room[,] levelRooms;
    public GameObject roomWhiteObj;

    int m_RoomSpriteWidth;
    int m_RoomSpriteHeight;

    public void Setup(Vector2 _WorldSize, Transform _miniMapTransform) {

        m_RoomSpriteWidth = WorldInstance.instance.roomTilesWide;
        m_RoomSpriteHeight = WorldInstance.instance.roomTilesHigh;

        m_TakenPositions.Clear();

        m_MiniMapTransform = _miniMapTransform;

        m_NumberOfRooms = Mathf.RoundToInt((_WorldSize.x) * (_WorldSize.y));
        //if (m_NumberOfRooms >= (_WorldSize.x) * (_WorldSize.y)){ // make sure we dont try to make more rooms than can fit in our grid
		//	m_NumberOfRooms = Mathf.RoundToInt((_WorldSize.x) * (_WorldSize.y));
		//}
		m_GridSizeX = Mathf.RoundToInt(_WorldSize.x);
		m_GridSizeY = Mathf.RoundToInt(_WorldSize.y);

        CreateRooms(); //lays out the actual map
		SetRoomDoors(); //assigns the doors where rooms would connect
		DrawMiniMap(); //instantiates objects to make up a map
	}

    void CreateRooms()
    {
        levelRooms = new Room[m_GridSizeX, m_GridSizeY];
        levelRooms[0, 0] = new Room(Vector2.zero, 1);
        m_TakenPositions.Insert(0, Vector2.zero);

        Vector2 checkPos = Vector2.zero;
        Debug.Log("Grid Size: " + m_GridSizeX + ", " + m_GridSizeY);
        for (int x=0; x < m_GridSizeX; x++)
        {
            for (int y = 0; y < m_GridSizeY; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                checkPos.x = x;
                checkPos.y = y;
                levelRooms[x, y] = new Room(checkPos, 0);
                Debug.Log("LevelRooms Length: " + levelRooms.Length);
                m_TakenPositions.Insert(0,checkPos);
            }
        }
        Debug.Log("LevelRooms Length: " + levelRooms.Length);
    }

    //void CreateRooms(){
    //       //setup
    //       levelRooms = new Room[m_GridSizeX * 2, m_GridSizeY * 2];
    //       levelRooms[m_GridSizeX,m_GridSizeY] = new Room(Vector2.zero, 1);
    //       m_TakenPositions.Insert(0,Vector2.zero);
    //	Vector2 checkPos = Vector2.zero;
    //	//magic numbers
    //	float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
    //	//add rooms
    //	for (int i =0; i < m_NumberOfRooms -1; i++){
    //		float randomPerc = ((float) i) / (((float)m_NumberOfRooms - 1));
    //		randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
    //		//grab new position
    //		checkPos = NewPosition();
    //		//test new position
    //		if (NumberOfNeighbors(checkPos, m_TakenPositions) > 1 && Random.value > randomCompare){
    //			int iterations = 0;
    //			do{
    //				checkPos = SelectiveNewPosition();
    //				iterations++;
    //			}while(NumberOfNeighbors(checkPos, m_TakenPositions) > 1 && iterations < 100);
    //			if (iterations >= 50)
    //				print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, m_TakenPositions));
    //		}
    //           //finalize position
    //           levelRooms[(int) checkPos.x + m_GridSizeX, (int) checkPos.y + m_GridSizeY] = new Room(checkPos, 0);
    //           m_TakenPositions.Insert(0,checkPos);
    //	}	
    //}
    //Vector2 NewPosition(){
    //	int x = 0, y = 0;
    //	Vector2 checkingPos = Vector2.zero;
    //	do{
    //		int index = Mathf.RoundToInt(Random.value * (m_TakenPositions.Count - 1)); // pick a random room
    //		x = (int)m_TakenPositions[index].x;//capture its x, y position
    //		y = (int)m_TakenPositions[index].y;
    //		bool UpDown = (Random.value < 0.5f);//randomly pick wether to look on hor or vert axis
    //		bool positive = (Random.value < 0.5f);//pick whether to be positive or negative on that axis
    //		if (UpDown){ //find the position bnased on the above bools
    //			if (positive){
    //				y += 1;
    //			}else{
    //				y -= 1;
    //			}
    //		}else{
    //			if (positive){
    //				x += 1;
    //			}else{
    //				x -= 1;
    //			}
    //		}
    //		checkingPos = new Vector2(x,y);
    //	}while (m_TakenPositions.Contains(checkingPos) || x >= m_GridSizeX || x < -m_GridSizeX || y >= m_GridSizeY || y < -m_GridSizeY); //make sure the position is valid
    //	return checkingPos;
    //}
    //Vector2 SelectiveNewPosition(){ // method differs from the above in the two commented ways
    //	int index = 0, inc = 0;
    //	int x =0, y =0;
    //	Vector2 checkingPos = Vector2.zero;
    //	do{
    //		inc = 0;
    //		do{ 
    //			//instead of getting a room to find an adject empty space, we start with one that only 
    //			//as one neighbor. This will make it more likely that it returns a room that branches out
    //			index = Mathf.RoundToInt(Random.value * (m_TakenPositions.Count - 1));
    //			inc ++;
    //		}while (NumberOfNeighbors(m_TakenPositions[index], m_TakenPositions) > 1 && inc < 100);
    //		x = (int)m_TakenPositions[index].x;
    //		y = (int)m_TakenPositions[index].y;
    //		bool UpDown = (Random.value < 0.5f);
    //		bool positive = (Random.value < 0.5f);
    //		if (UpDown){
    //			if (positive){
    //				y += 1;
    //			}else{
    //				y -= 1;
    //			}
    //		}else{
    //			if (positive){
    //				x += 1;
    //			}else{
    //				x -= 1;
    //			}
    //		}
    //		checkingPos = new Vector2(x,y);
    //	}while (m_TakenPositions.Contains(checkingPos) || x >= m_GridSizeX || x < -m_GridSizeX || y >= m_GridSizeY || y < -m_GridSizeY);
    //	if (inc >= 100){ // break loop if it takes too long: this loop isnt garuanteed to find solution, which is fine for this
    //		print("Error: could not find position with only one neighbor");
    //	}
    //	return checkingPos;
    //}
    int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions){
		int ret = 0; // start at zero, add 1 for each side there is already a room
		if (usedPositions.Contains(checkingPos + Vector2.right)){ //using Vector.[direction] as short hands, for simplicity
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.left)){
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.up)){
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.down)){
			ret++;
		}
		return ret;
	}
	void DrawMiniMap(){
		foreach (Room room in levelRooms)
        {
			if (room == null){
				continue; //skip where there is no room
			}
			Vector2 drawPos = room.gridPos;
			drawPos.x = drawPos.x * m_RoomSpriteWidth + m_RoomSpriteWidth/2;//aspect ratio of map sprite
			drawPos.y = drawPos.y * m_RoomSpriteHeight + m_RoomSpriteHeight/2;
			//create map obj and assign its variables
			MapSpriteSelector mapper = Object.Instantiate(roomWhiteObj, drawPos, Quaternion.identity).GetComponent<MapSpriteSelector>();
			mapper.type = room.type;
			mapper.up = room.doorTop;
			mapper.down = room.doorBot;
			mapper.right = room.doorRight;
			mapper.left = room.doorLeft;
			mapper.gameObject.transform.parent = m_MiniMapTransform;
		}
	}
	void SetRoomDoors(){
		for (int x = 0; x < ((m_GridSizeX)); x++){
			for (int y = 0; y < ((m_GridSizeY)); y++){
				if (levelRooms[x,y] == null){
					continue;
				}

				if (y - 1 < 0){ //check above
                    levelRooms[x,y].doorBot = false;
				}else{
                    levelRooms[x,y].doorBot = (levelRooms[x,y-1] != null);
				}
				if (y + 1 >= m_GridSizeY){ //check bellow
                    levelRooms[x,y].doorTop = false;
				}else{
                    levelRooms[x,y].doorTop = (levelRooms[x,y+1] != null);
				}
				if (x - 1 < 0){ //check left
                    levelRooms[x,y].doorLeft = false;
				}else{
                    levelRooms[x,y].doorLeft = (levelRooms[x - 1,y] != null);
				}
				if (x + 1 >= m_GridSizeX){ //check right
                    levelRooms[x,y].doorRight = false;
				}else{
                    levelRooms[x,y].doorRight = (levelRooms[x+1,y] != null);
				}
			}
		}
	}
}
