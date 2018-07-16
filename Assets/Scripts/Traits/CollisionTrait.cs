using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CollisionInfo
{
    public bool above, below;
    public bool left, right;
    public Vector2 velocityOld;

    public void Reset()
    {
        above = below = false;
        left = right = false;
    }
}

[RequireComponent(typeof(BoxCollider2D))]
public class CollisionTrait : MonoBehaviour
{

    public LayerMask collisionMask;

    public float skinWidth = 0.015f;
    const float dstBetweenRays = .25f;
    [HideInInspector]
    public int horizontalRayCount;
    [HideInInspector]
    public int verticalRayCount;
    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;

    [HideInInspector]
    public new BoxCollider2D collider;
    public RaycastOrigins raycastOrigins;


    public CollisionInfo collisions;

    // Use this for initialization
    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        collisions = new CollisionInfo();
    }

    void Start()
    {
        CalculateRaySpacing();
    }

    public void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public void HorizontalCollisions(ref Vector3 _velocity)
    {
        float directionX = Mathf.Sign(_velocity.x);
        float rayLength = Mathf.Abs(_velocity.x) + skinWidth;

        if (Mathf.Abs(_velocity.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        int hitCount = 0;
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(
                rayOrigin,
                Vector2.right * directionX,
                Color.red);

            if (hit)
            {
                if (hit.distance == 0)
                {
                    continue;
                }

                hitCount++;

                _velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                if (hitCount >= Mathf.FloorToInt(horizontalRayCount * 0.5f))
                {
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    public void VerticalCollisions(ref Vector3 _velocity)
    {
        float directionY = Mathf.Sign(_velocity.y);
        float rayLength = Mathf.Abs(_velocity.y) + skinWidth;

        int hitCount = 0;
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + _velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(
                rayOrigin,
                Vector2.up * directionY,
                Color.red);

            if (hit)
            {
                if (hit.distance == 0)
                {
                    continue;
                }

                hitCount++;

                _velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (hitCount >= Mathf.FloorToInt(verticalRayCount * 0.5f))
                {
                    collisions.left = directionY == -1;
                    collisions.right = directionY == 1;
                }
            }
        }
    }

}
