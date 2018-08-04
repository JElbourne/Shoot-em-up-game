using System.Collections.Generic;
using UnityEngine;

namespace Rts2DEngine
{
    namespace Map
    {
        public class LevelGeneration : MonoBehaviour
        {

            List<Vector2> m_TakenPositions = new List<Vector2>();
            int m_GridSizeX, m_GridSizeY;
            Transform m_MiniMapTransform;

            public Room[,] levelRooms;
            public GameObject roomWhiteObj;

            int m_RoomSpriteWidth;
            int m_RoomSpriteHeight;

            public void Setup(Vector2 _WorldSize, Transform _miniMapTransform)
            {

                m_RoomSpriteWidth = MapInstance.instance.roomTilesWide;
                m_RoomSpriteHeight = MapInstance.instance.roomTilesHigh;

                m_TakenPositions.Clear();

                m_MiniMapTransform = _miniMapTransform;

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
                for (int x = 0; x < m_GridSizeX; x++)
                {
                    for (int y = 0; y < m_GridSizeY; y++)
                    {
                        if (x == 0 && y == 0)
                            continue;

                        checkPos.x = x;
                        checkPos.y = y;
                        levelRooms[x, y] = new Room(checkPos, 0);
                        Debug.Log("LevelRooms Length: " + levelRooms.Length);
                        m_TakenPositions.Insert(0, checkPos);
                    }
                }
                Debug.Log("LevelRooms Length: " + levelRooms.Length);
            }

            int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
            {
                int ret = 0; // start at zero, add 1 for each side there is already a room
                if (usedPositions.Contains(checkingPos + Vector2.right))
                { //using Vector.[direction] as short hands, for simplicity
                    ret++;
                }
                if (usedPositions.Contains(checkingPos + Vector2.left))
                {
                    ret++;
                }
                if (usedPositions.Contains(checkingPos + Vector2.up))
                {
                    ret++;
                }
                if (usedPositions.Contains(checkingPos + Vector2.down))
                {
                    ret++;
                }
                return ret;
            }

            void DrawMiniMap()
            {
                foreach (Room room in levelRooms)
                {
                    if (room == null)
                    {
                        continue; //skip where there is no room
                    }
                    Vector2 drawPos = room.gridPos;
                    drawPos.x = drawPos.x * m_RoomSpriteWidth + m_RoomSpriteWidth / 2;//aspect ratio of map sprite
                    drawPos.y = drawPos.y * m_RoomSpriteHeight + m_RoomSpriteHeight / 2;
                    //create map obj and assign its variables
                    MiniMapSpriteSelector mapper = Object.Instantiate(roomWhiteObj, drawPos, Quaternion.identity).GetComponent<MiniMapSpriteSelector>();
                    mapper.type = room.type;
                    mapper.up = room.doorTop;
                    mapper.down = room.doorBot;
                    mapper.right = room.doorRight;
                    mapper.left = room.doorLeft;
                    mapper.gameObject.transform.parent = m_MiniMapTransform;
                }
            }

            void SetRoomDoors()
            {
                for (int x = 0; x < ((m_GridSizeX)); x++)
                {
                    for (int y = 0; y < ((m_GridSizeY)); y++)
                    {
                        if (levelRooms[x, y] == null)
                        {
                            continue;
                        }

                        if (y - 1 < 0)
                        { //check above
                            levelRooms[x, y].doorBot = false;
                        }
                        else
                        {
                            levelRooms[x, y].doorBot = (levelRooms[x, y - 1] != null);
                        }
                        if (y + 1 >= m_GridSizeY)
                        { //check bellow
                            levelRooms[x, y].doorTop = false;
                        }
                        else
                        {
                            levelRooms[x, y].doorTop = (levelRooms[x, y + 1] != null);
                        }
                        if (x - 1 < 0)
                        { //check left
                            levelRooms[x, y].doorLeft = false;
                        }
                        else
                        {
                            levelRooms[x, y].doorLeft = (levelRooms[x - 1, y] != null);
                        }
                        if (x + 1 >= m_GridSizeX)
                        { //check right
                            levelRooms[x, y].doorRight = false;
                        }
                        else
                        {
                            levelRooms[x, y].doorRight = (levelRooms[x + 1, y] != null);
                        }
                    }
                }
            }
        }
    }
}

