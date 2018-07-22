using UnityEngine;
using System.Collections.Generic;

public class TileGraph: MonoBehaviour
{
    public Transform player;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public bool displayGridGizmos = true;
    PathNode[,] grid;

    float m_NodeDiameter;
    int m_GridSizeX;
    int m_GridSizeY;

    public void Setup()
    {
        m_NodeDiameter = nodeRadius * 2;
        m_GridSizeX = Mathf.RoundToInt(gridWorldSize.x / m_NodeDiameter);
        m_GridSizeY = Mathf.RoundToInt(gridWorldSize.y / m_NodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return m_GridSizeX * m_GridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new PathNode[m_GridSizeX, m_GridSizeY];
        Vector3 m_WorldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y/2;

        for (int x=0; x < m_GridSizeX; x++)
        {
            for (int y = 0; y < m_GridSizeY; y++)
            {
                Vector3 m_WorldPoint = m_WorldBottomLeft + Vector3.right * (x * m_NodeDiameter + nodeRadius) + Vector3.up * (y * m_NodeDiameter + nodeRadius);
                bool walkable = (Physics2D.CircleCastAll(m_WorldPoint, nodeRadius, Vector3.zero, 0f, unwalkableMask).Length == 0);
                grid[x, y] = new PathNode(walkable, m_WorldPoint, x, y);
            }
        }
    }

    public List<PathNode> GetNeighbours(PathNode node)
    {
        List<PathNode> neighbours = new List<PathNode>();
        for (int x = -1; x <= 1; x++ )
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < m_GridSizeX && checkY >= 0 && checkY < m_GridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public PathNode NodeFromWorldPoint( Vector3 _worldPosition)
    {
        float percentX = (_worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (_worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((m_GridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((m_GridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0));

        if(grid != null && displayGridGizmos)
        {
            PathNode playerNode = NodeFromWorldPoint(Vector3.zero);
            if (player)
            {
                playerNode = NodeFromWorldPoint(player.position);
            }
            foreach(PathNode n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (playerNode != null && playerNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(n.coord, Vector3.one * (m_NodeDiameter - .1f));
            }
        }
    }
}
