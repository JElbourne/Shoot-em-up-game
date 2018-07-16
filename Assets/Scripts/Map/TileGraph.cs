using System.Collections.Generic;
using UnityEngine;

public class PathNode<Tile>
{
    public Tile data;
    public PathEdge<Tile>[] edges;
}

public class PathEdge<Tile>
{

    public float cost;
    public PathNode<Tile> node;
}

public class TileGraph
{
    public Dictionary<Tile, PathNode<Tile>> nodes;

    public TileGraph(WorldInstance _world)
    {
        nodes = new Dictionary<Tile, PathNode<Tile>>();

        foreach(Tile _tile in _world.mapTileData.Values)
        {
            if (!_tile.hasCollider())
            {
                PathNode<Tile> node = new PathNode<Tile>();
                node.data = _tile;
                node.edges = DetermineEdges(_world, _tile, node);
                nodes.Add(_tile, node);
            }
        }

        Debug.Log("TileGraph created: " + nodes.Count + " nodes.");
    }

    PathEdge<Tile>[] DetermineEdges(WorldInstance _world, Tile _tile, PathNode<Tile> _node)
    {
        List<PathEdge<Tile>> edges = new List<PathEdge<Tile>>();

        Tile[] m_Neighbours = _world.getTileNeighbours(_tile.getCoord());

        for (int i=0; i < m_Neighbours.Length; i++)
        {
           if (m_Neighbours[i] != null && !m_Neighbours[i].hasCollider())
            {
                PathEdge<Tile> edge = new PathEdge<Tile>();
                edge.cost = m_Neighbours[i].movementCost;
                edge.node = nodes[m_Neighbours[i]];
                edges.Add(edge);
            }
        }
        return edges.ToArray();
    }
}
