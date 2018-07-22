using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    PathRequestManager requestManager;
    TileGraph graph;

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        graph = GetComponent<TileGraph>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        PathNode startNode = graph.NodeFromWorldPoint(startPos);
        PathNode targetNode = graph.NodeFromWorldPoint(targetPos);

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<PathNode> openSet = new Heap<PathNode>(graph.MaxSize);
            HashSet<PathNode> closedSet = new HashSet<PathNode>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                PathNode currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    sw.Stop();
                    print("Path found: " + sw.ElapsedMilliseconds + "ms");
                    pathSuccess = true;
                    break;
                }

                foreach (PathNode neighbour in graph.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<PathNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i=1; i< path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].coord);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(PathNode _nodeA, PathNode _nodeB)
    {
        int distX = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);
        int distY = Mathf.Abs(_nodeA.gridY - _nodeB.gridY);
        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);
    }
}

//using UnityEngine;
//using System.Collections.Generic;
//using System.Linq;
//using Priority_Queue;

//public class AStarPath {

//    Queue<Vector3> m_Path;

//    public AStarPath(Vector3 _tileStart, Vector3 _tileEnd)
//    {
//        WorldInstance m_World = WorldInstance.instance;

//        if (m_World == null)
//            Debug.LogWarning("World is not created yet. Could not find instance.");

//        if (m_World.tileGraph == null)
//            m_World.tileGraph = new TileGraph(m_World);

//        Dictionary<Vector3, PathNode<Vector3>> nodes = m_World.tileGraph.nodes;

//        //Debug.Log("A tileEnd: " + _tileEnd);
//        //Debug.Log("A tileStart: " + _tileStart);

//        if (!nodes.ContainsKey(_tileStart))
//        {
//            Debug.LogWarning("AStarPath: The starting tile isn't in the list of Nodes.");
//            return;
//        }

//        if (!nodes.ContainsKey(_tileEnd))
//        {
//            Debug.LogWarning("AStarPath: The ending tile isn't in the list of Nodes.");
//            return;
//        }

//        PathNode<Vector3> m_Start = nodes[_tileStart];
//        PathNode<Vector3> m_Goal = nodes[_tileEnd];

//        //Debug.Log("Setup m_Start & m_Goal");

//        List<PathNode<Vector3>> m_ClosedSet = new List<PathNode<Vector3>>();

//        SimplePriorityQueue<PathNode<Vector3>> m_OpenSet = new SimplePriorityQueue<PathNode<Vector3>>();
//        m_OpenSet.Enqueue(m_Start, 0);
           
//        Dictionary<PathNode<Vector3>, PathNode<Vector3>> m_CameFrom = new Dictionary<PathNode<Vector3>, PathNode<Vector3>>();

//        Dictionary<PathNode<Vector3>, float> m_GScore = new Dictionary<PathNode<Vector3>, float>();
//        foreach(PathNode<Vector3> n in nodes.Values)
//        {
//            m_GScore[n] = Mathf.Infinity;
//        }
//        m_GScore[m_Start] = 0f;

//        Dictionary<PathNode<Vector3>, float> m_FScore = new Dictionary<PathNode<Vector3>, float>();
//        foreach (PathNode<Vector3> n in nodes.Values)
//        {
//            m_FScore[n] = Mathf.Infinity;
//        }
//        m_FScore[m_Start] = heuristicCostEstimate(m_Start, m_Goal);

//        while (m_OpenSet.Count > 0)
//        {
//            PathNode<Vector3> m_Current = m_OpenSet.Dequeue();
//            //Debug.Log("================================ ");
//            //Debug.Log("Checking new Current: " + m_Current.coord + " with FScore: " + m_FScore[m_Current]);
//            //Debug.Log("================================ ");
//            //Debug.Log(" ");

//            if (m_Current.coord == m_Goal.coord)
//            {
//                // We have reached our gloal. Let's convert this into an actual sequence of tiles to walk on.
//                Debug.Log("Reached Goal -> next step: Reconstruct Path");
//                ReconstructPath(m_CameFrom, m_Current);
//                return;
//            }

//            m_ClosedSet.Add(m_Current);

//            foreach(PathEdge<Vector3> m_NeighbourEdge in m_Current.edges)
//            {
//                PathNode<Vector3> m_Neighbour = m_NeighbourEdge.node;
//                if(m_ClosedSet.Contains(m_Neighbour) == true)
//                {
//                    continue;
//                }

//                float m_TentativeGScore = m_GScore[m_Current] + distBetween(m_Current, m_Neighbour);

//                if(m_OpenSet.Contains(m_Neighbour) && m_TentativeGScore >= m_GScore[m_Neighbour] )
//                {
//                    continue;
//                }

//                m_CameFrom[m_Neighbour] = m_Current;
//                m_GScore[m_Neighbour] = m_TentativeGScore;
//                m_FScore[m_Neighbour] = m_GScore[m_Neighbour] + heuristicCostEstimate(m_Neighbour, m_Goal);

//                if(m_OpenSet.Contains(m_Neighbour) == false)
//                {
//                    //Debug.Log("Adding m_Neighbour: " + m_Neighbour.coord + " with FScore: " + m_FScore[m_Neighbour]);
//                    //Debug.Log("Added to OpenSet");
//                    if (m_Neighbour.coord == m_Goal.coord)
//                    {
//                        //Debug.Log("Just Added Goal to OpenSet");
//                    }
//                    m_OpenSet.Enqueue(m_Neighbour, m_FScore[m_Neighbour]);
//                }
//            } // foreach neighbour
//        } // while

//        Debug.LogWarning("There is no path from start to goal");
//        // if we reach here, it means that we've burned through the entire
//        // OpenSet without ever reaching a point were current == goal.
//        // This happens when there is no path from start to goal.
//    }

//    float heuristicCostEstimate(PathNode<Vector3> _pointA, PathNode<Vector3> _pointB)
//    {
//        return Mathf.Sqrt(
//            Mathf.Pow(Mathf.Abs(_pointA.coord.x - _pointB.coord.x), 2) +
//            Mathf.Pow(Mathf.Abs(_pointA.coord.y - _pointB.coord.y), 2)
//            );
//    }

//    float distBetween(PathNode<Vector3> _pointA, PathNode<Vector3> _pointB)
//    {
//        return Vector3.Distance(_pointA.coord, _pointB.coord);
//        //if(Mathf.Abs(_pointA.coord.x - _pointB.coord.x) + Mathf.Abs(_pointA.coord.y - _pointA.coord.y) == 1)
//        //{
//        //    return 1f;
//        //}

//        //if(Mathf.Abs(_pointA.coord.x - _pointB.coord.x) == 1 &&
//        //    Mathf.Abs(_pointA.coord.y - _pointA.coord.y) == 1)
//        //{
//        //    return 1.41421356247f;
//        //}

//        //return Mathf.Sqrt(
//        //    Mathf.Pow(_pointA.coord.x - _pointB.coord.x, 2) +
//        //    Mathf.Pow(_pointA.coord.y - _pointB.coord.y, 2)
//        //    );
//    }

//    void ReconstructPath(
//        Dictionary<PathNode<Vector3>, PathNode<Vector3>> _cameFrom,
//        PathNode<Vector3> _current)
//    {
//        Queue<Vector3> m_TotalPath = new Queue<Vector3>();
//        m_TotalPath.Enqueue(_current.coord);

//        while(_cameFrom.ContainsKey(_current))
//        {
//            _current = _cameFrom[_current];
//            m_TotalPath.Enqueue(_current.coord);
//        }

//        m_Path = new Queue<Vector3>(m_TotalPath.Reverse());
//        Debug.Log("Created Path with length: " + this.Length());

//    }

//    public Vector3 Dequeue()
//    {
//        return m_Path.Dequeue();
//    }

//    public int Length()
//    {
//        if (m_Path == null)
//            return 0;

//        return m_Path.Count;
//    }
//}
