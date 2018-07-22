using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MoveTrait))]
public class PathfindingTrait : MonoBehaviour {

    public Transform target;

    MoveTrait m_MoveTrait;
    Vector3[] path;
    int targetIndex;
    Vector2 m_CurrentDirection;
    CharacterStats m_CharacterStats;

    private void Awake()
    {
        m_MoveTrait = GetComponent<MoveTrait>();
        m_CharacterStats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        if (target & path == null)
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newpath, bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            path = newpath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        float maxSpeed = m_CharacterStats.maxSpeed.value;

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, maxSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

    //MoveTrait m_MoveTrait;
    //AStarPath m_AStarPath;
    //Vector3 m_NextCoord;
    //Vector3 m_CurrentEndCoord;
    //Vector2 m_CurrentDirection;
    //float m_CurrentDistance;
    //IEnumerator m_UpdateMovement;

    //bool m_Moving = false;

    //void Start () {
    //    m_MoveTrait = GetComponent<MoveTrait>();
    //    m_NextCoord = transform.position;
    //    m_UpdateMovement = UpdateMovement();
    //}

    //private void Update()
    //{
    //    if (m_Moving)
    //    {
    //        Vector3 m_Heading = (m_NextCoord - transform.position);
    //        Vector3 m_Direction = m_Heading / m_Heading.magnitude;
    //        m_CurrentDirection.x = m_Direction.x;
    //        m_CurrentDirection.y = m_Direction.y;
    //        Debug.Log("Calling m_MoveTrait with direction: " + m_Direction.x + ", " + m_Direction.y);
    //        Debug.Log("m_CurrentDistance: " + m_CurrentDistance);
    //        m_CurrentDistance = Vector3.Distance(m_NextCoord, transform.position);
    //        m_MoveTrait.Move(m_CurrentDirection);
    //    }  
    //}

    //public void Move(Vector3 _endPosition)
    //{
    //    if (m_Moving)
    //    {
    //        Debug.Log("Stopping previous Coroutine.");
    //        StopCoroutine(m_UpdateMovement);
    //    }

    //    Debug.Log("Called PathfindingTrait Move");
    //    m_Moving = false; // Set moving to false as we set things up and do our checks.

    //    SetTranformToAbsTilePostion();

    //    Vector2 m_MapTilePosition = WorldInstance.instance.getTilePosition(_endPosition);
    //    Vector3 m_EndCoord = new Vector3(m_MapTilePosition.x, m_MapTilePosition.y, WorldInstance.instance.currentLevel);
    //    Vector3 m_StartCoord = new Vector3(transform.position.x, transform.position.y, WorldInstance.instance.currentLevel);

    //    if(!WorldInstance.instance.isCoordInMapTileData(m_EndCoord))
    //    {
    //        Debug.LogWarning("m_EndCoord is not a coord in Map Tile Data.");
    //        return;
    //    }

    //    if (!WorldInstance.instance.isCoordInMapTileData(m_StartCoord))
    //    {
    //        Debug.LogWarning("m_StartCoord is not a coord in Map Tile Data.");
    //        return;
    //    }

    //    if (m_CurrentEndCoord == m_EndCoord)
    //    {
    //        Debug.LogWarning("m_CurrentEndCoord is already at m_EndCoord.");
    //        return;
    //    }

    //    if (m_StartCoord == m_EndCoord)
    //    {
    //        Debug.LogWarning("m_StartCoord == m_EndCoord");
    //        return;
    //    }

    //    if (m_AStarPath == null)
    //    {
    //        Debug.Log("Creating new instance of AStarPath");
    //        m_AStarPath = new AStarPath(m_StartCoord, m_EndCoord);
    //    }

    //    Debug.Log("Starting Coroutine");
    //    m_CurrentEndCoord = m_EndCoord;
    //    StartCoroutine(m_UpdateMovement);
    //}

    //void SetTranformToAbsTilePostion()
    //{
    //    Vector3 m_AbsPos = new Vector3(
    //        Mathf.Round(transform.position.x),
    //        Mathf.Round(transform.position.y),
    //        transform.position.z);

    //    transform.position = m_AbsPos;
    //    Debug.Log("Set Transofrm position to Absolute: " + m_AbsPos.x + ", " + m_AbsPos.y);
    //}

    //IEnumerator UpdateMovement()
    //{
    //    Debug.Log("Setting move to True");
    //    m_Moving = true;

    //    if (m_AStarPath == null || m_AStarPath.Length() == 0)
    //    {
    //        Debug.LogWarning("Nothing set for m_AStarPath");
    //        m_AStarPath = null;
    //        m_Moving = false;
    //        yield break;
    //    }
    //    else if (m_MoveTrait == null)
    //    {
    //        Debug.LogWarning("There is no reference to m_MoveTrait");
    //        m_Moving = false;
    //        yield break;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Starting Path Loop");
    //        int m_PathLength = m_AStarPath.Length();
    //        for (int i=0; i < m_PathLength; i++)
    //        {
    //            m_NextCoord = m_AStarPath.Dequeue();
    //            m_NextCoord.z = transform.position.z; // Previously stored the level data in 'z' so need to reset.
    //            if (m_NextCoord == transform.position)
    //                continue;

    //            Debug.Log("Setting m_NextCoord: " + m_NextCoord.x + ", " + m_NextCoord.y);

    //            yield return new WaitWhile(() => m_CurrentDistance > 0.5f);
    //            //yield return new WaitForFixedUpdate();

    //        }
    //        SetTranformToAbsTilePostion();
    //    }
    //    m_Moving = false;
    //    Debug.Log("Coroutine is complete. Setting move to False");
    //}
}
