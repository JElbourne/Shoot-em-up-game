using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewGenerator : MonoBehaviour {

    GameController m_Game;
    WorldInstance m_World;
    PlayerController m_PlayerController;
    Dictionary<Vector3, float> m_LitCoords = new Dictionary<Vector3, float>();
    //Dictionary<Vector3, GameObject> m_EntityCoords = new Dictionary<Vector3, GameObject>();

    int m_VisionSections = 8;
    int m_LightLevel = 1;
    int[,] multi;
    int m_mMapLevel;

    private void Awake()
    {
        m_World = GetComponent<WorldInstance>();
        m_Game = FindObjectOfType<GameController>();
    }

    private void Start()
    {
        m_mMapLevel = m_World.currentLevel;
    }

    public bool isTileLit(Vector3 coord)
    {
        return m_LitCoords.ContainsKey(coord);
    }

    public Dictionary<Vector3, float> GetAllLitCoords()
    {
        return m_LitCoords;
    }

    public void GenerateFOV()
    {
        ClearFOV();
        m_mMapLevel = m_World.currentLevel;
        foreach (GameObject entityGo in m_Game.entities)
        {
            EntityController entity = entityGo.GetComponent<EntityController>();
            int[] entityTileCoord = entity.getTileCoord();
            if (entity.lightLevel <= 0)
            {
                continue;
                // m_EntityCoords[entityGo.transform.position] = entityGo;
            } else
            {
                m_LightLevel = entity.lightLevel;
                SetupMulti(entity.limitedLighting, entityGo);

                for( int section = 0; section < m_VisionSections; section++)
                {
                    CastLight(
                        entityTileCoord[0],
                        entityTileCoord[1],
                        1,
                        1.0f,
                        0.0f,
                        m_LightLevel,
                        multi[0, section],
                        multi[1, section],
                        multi[2, section],
                        multi[3, section],
                        0
                        );
                }

                SetLit(new Vector3(
                    Mathf.Round(entityGo.transform.position.x),
                    Mathf.Round(entityGo.transform.position.y),
                    m_mMapLevel),
                    100f);
            }
        }

        RedrawFOV();
    }

    void SetupMulti(bool forwardOnly, GameObject entityGo)
    {
        m_VisionSections = 8;
        multi = new int[4, 8] {
                        { 1, 0, 0, -1, -1, 0, 0, 1 },
                        { 0, 1, -1, 0, 0, -1, 1, 0},
                        { 0, 1, 1, 0, 0, -1, -1, 0 },
                        {1, 0, 0, 1, -1, 0, 0, -1 },
                        };

        if (forwardOnly)
        {
            m_VisionSections = 2;
            m_LightLevel = Mathf.FloorToInt(m_LightLevel / 1.2f);
            float rot = entityGo.transform.rotation.eulerAngles.z + 90.0f;
            if (rot > 225 && rot <= 315)
            {
                // Angle == 270
                multi = new int[4, 2] { { 1, -1 }, { 0, 0 }, { 0, 0 }, { 1, 1 } };
            }
            else if (rot > 45 && rot <= 135)
            {
                // Angle == 90
                multi = new int[4, 2] { { -1, 1 }, { 0, 0 }, { 0, 0 }, { -1, -1 } };
            }
            else if (rot > 135 && rot <= 225)
            {
                // Angle == 180
                multi = new int[4, 2] { { 0, 0 }, { 1, 1 }, { -1, 1 }, { 0, 0 } };
            }
            else
            {
                // Angle == 0
                multi = new int[4, 2] { { 0, 0 }, { -1, -1 }, { 1, -1 }, { 0, 0 } };
            }
        }
    }

    void SetLit(Vector3 _coord, float _opacity)
    {
        if (m_World.isCoordInMapTileData(_coord))
        {
            m_LitCoords[_coord] = _opacity;
        }
    }

    void ClearFOV()
    {
        foreach(Vector3 coord in m_LitCoords.Keys)
        {
            Tile tile = m_World.getMapTileData(coord);
            if (tile != null)
            {
                tile.SetOpacity(0f);
                // tile.DisableVisible();
            } 
        }
        m_LitCoords.Clear();
    }

    void RedrawFOV()
    {
        foreach (Vector3 coord in m_LitCoords.Keys)
        {
            Tile tile = m_World.getMapTileData(coord);
            if (tile != null)
            {
                tile.SetOpacity(m_LitCoords[coord]);
                //tile.EnableVisible();
            }   
        }
    }

    bool lightHitsObstacle(Vector3 coord)
    {
        Tile m_TileData = m_World.getMapTileData(coord);
        if (m_TileData != null)
        {
            return m_TileData.willBlockLight();
        }
        return true;
    }

    void CastLight(int _cx, int _cy, int _row, float _start, float _end, int _radius, int _xx, int _xy, int _yx, int _yy, int _id)
    {
        float newStart = _start;
        float m_Opacity = 100f;

        if (_start < _end)
        {
            return;
        }

        int radiusSquared = _radius * _radius;

        for (int i = _row; i < _radius + 1; i++ )
        {
            int dx = -i - 1;
            int dy =  -i;
            bool blocked = false;
            while (dx <= 0) {
                dx += 1;
                int x = _cx + (dx * _xx + dy * _xy);
                int y = _cy + (dx * _yx + dy * _yy);
                float lSlope = (dx - 0.5f) / (dy + 0.5f);
                float rSlope = (dx + 0.5f) / (dy - 0.5f);
                if (_start < rSlope)
                {
                    continue;
                } else if(_end > lSlope)
                {
                    break;
                } else
                {
                    if ((dx*dx + dy*dy) < radiusSquared)
                    {
                        if ((dx * dx + dy * dy) > 14)
                        {
                            m_Opacity = 20f;
                        } else if ((dx * dx + dy * dy) > 12)
                        {
                            m_Opacity = 40f;
                        } else if ((dx * dx + dy * dy) > 10)
                        {
                            m_Opacity = 60f;
                        } else
                        {
                            m_Opacity = 100f;
                        }
                        SetLit(new Vector3(x, y, m_mMapLevel), m_Opacity);
                    }

                    if (blocked)
                    {
                        if (lightHitsObstacle(new Vector3(x, y, m_mMapLevel)))
                        {
                            newStart = rSlope;
                            continue;
                        } else
                        {
                            blocked = false;
                            _start = newStart;
                        }
                    } else
                    {
                        if (lightHitsObstacle(new Vector3(x, y, m_mMapLevel)) && i < _radius)
                        {
                            blocked = true;
                            CastLight(_cx, _cy, i + 1, _start, lSlope, _radius, _xx, _xy, _yx, _yy, _id + 1);
                            newStart = rSlope;
                        }
                    }
                }
            }

            if (blocked)
            {
                break;
            }
             
        }
    }
}
