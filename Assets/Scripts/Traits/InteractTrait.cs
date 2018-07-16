using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class InteractTrait : MonoBehaviour {

    public LayerMask interatableMask;

    public float skinWidth = 0.015f;
    public float defaultRayLength = 0.5f;

    const float dstBetweenRays = .25f;
    int m_RayCount;
    float m_RaySpacing;

    BoxCollider2D m_Collider;
    MoveTrait m_MoveTrait;
    Vector2 m_FacingDirection = new Vector2(1,0);

    private void Awake()
    {
        m_Collider = GetComponent<BoxCollider2D>();
        m_MoveTrait = GetComponent<MoveTrait>();
    }

    public void Interact()
    {
        if (m_MoveTrait == null)
        {
            Debug.LogWarning("Missing access to Move Trait from the Interact Trait");
            return;
        }

        m_FacingDirection = m_MoveTrait.GetFacingDirection();
        CalculateRaySpacing();

        Vector2[] m_RayVectors = SetRaycastOrigins();
        float m_RayLength = DetermineRayLength(m_MoveTrait.lastVelocity);

        for (int i = 0; i < m_RayCount; i++)
        {
            Vector2 m_RayOrigin = m_RayVectors[0] + m_RayVectors[2] * (m_RaySpacing * i);
            Vector2 m_RayDirection = m_RayVectors[1];
            RaycastHit2D hit = Physics2D.Raycast(m_RayOrigin, m_RayDirection, m_RayLength, interatableMask);
            
            Debug.DrawRay(m_RayOrigin, m_RayDirection, Color.blue);

            if (hit)
            {
                if (hit.distance == 0)
                {
                    continue;
                }
                InteractableTrait interactable = hit.collider.gameObject.GetComponent<InteractableTrait>();
                if (interactable)
                {
                    // Debug.Log("Interact Hit");
                    interactable.Interact();
                    break;
                }
            }
        }
    }

    float DetermineRayLength(Vector3 _velocity)
    {
        float m_RayLength = defaultRayLength;
        float m_Velocity = 0;

        if(m_FacingDirection.x == 0 || m_FacingDirection.x < m_FacingDirection.y)
        {
            m_Velocity = Mathf.Abs(_velocity.y);
        } else if (m_FacingDirection.y == 0 || m_FacingDirection.x > m_FacingDirection.y)
        {
            m_Velocity = Mathf.Abs(_velocity.x);
        }


        if (Mathf.Abs(m_Velocity) > skinWidth)
        {
            m_RayLength = m_Velocity + skinWidth;
        }

        return m_RayLength;
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = m_Collider.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        if (m_FacingDirection.x == 0)
        {
            m_RayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
            m_RaySpacing = bounds.size.y / (m_RayCount - 1);
        } else
        {
            m_RayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);
            m_RaySpacing = bounds.size.x / (m_RayCount - 1);
        } 
    }

    Vector2[] SetRaycastOrigins()
    {
        Bounds bounds = m_Collider.bounds;
        bounds.Expand(skinWidth * -2);

        if (m_FacingDirection.x > 0 && m_FacingDirection.y == 0)
        {
            Vector2 origin = new Vector2(bounds.max.x, bounds.min.y);
            return new Vector2[] { origin, Vector2.right, Vector2.up };
        }
        else if (m_FacingDirection.x < 0 && m_FacingDirection.y == 0)
        {
            Vector2 origin = new Vector2(bounds.min.x, bounds.min.y);
            return new Vector2[] { origin, Vector2.left, Vector2.up };
        }
        else if (m_FacingDirection.x == 0 && m_FacingDirection.y < 0)
        {
            Vector2 origin = new Vector2(bounds.min.x, bounds.min.y);
            return new Vector2[] { origin, Vector2.down, Vector2.right };
        }
        else
        {
            Vector2 origin = new Vector2(bounds.min.x, bounds.max.y);
            return new Vector2[] { origin, Vector2.up, Vector2.right };
        }
    }
}
