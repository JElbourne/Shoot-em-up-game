using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class InteractTrait : MonoBehaviour {

    public float skinWidth = 0.015f;
    public float rayLength = 0.5f;
    
    BoxCollider2D m_Collider;
    MoveTrait m_MoveTrait;
    Vector2 m_FacingDirection = new Vector2(1,0);
    List<Vector2> m_RayOrigins = new List<Vector2>();

    private void Awake()
    {
        m_Collider = GetComponent<BoxCollider2D>();
        m_MoveTrait = GetComponent<MoveTrait>();
    }

    public void Interact()
    {
        Vector2[] rayVectors = SetRaycastOrigins();

        RaycastHit2D hit = Physics2D.Raycast(rayVectors[0], rayVectors[1], rayLength);

        Debug.DrawRay(rayVectors[0], rayVectors[1], Color.blue);

        if (hit)
        {
            Debug.Log("Interact Hit");
            InteractableTrait interactable = hit.collider.gameObject.GetComponent<InteractableTrait>();
            if(interactable)
            {

                interactable.Interact();
            }
        }
    }

    Vector2[] SetRaycastOrigins()
    {
        if (m_MoveTrait)
        {
            m_FacingDirection = m_MoveTrait.GetCurrentDirection();
            Debug.Log(m_FacingDirection);
        }

        Bounds bounds = m_Collider.bounds;
        bounds.Expand(skinWidth * -2);

        if (m_FacingDirection.x > 0 && m_FacingDirection.y == 0)
        {
            Vector2 origin = new Vector2(bounds.max.x, transform.position.y);
            return new Vector2[] { origin, Vector2.right };
        }
        else if (m_FacingDirection.x < 0 && m_FacingDirection.y == 0)
        {
            Vector2 origin = new Vector2(bounds.min.x, transform.position.y);
            return new Vector2[] { origin, Vector2.left };
        }
        else if (m_FacingDirection.x == 0 && m_FacingDirection.y < 0)
        {
            Vector2 origin = new Vector2(transform.position.x, bounds.min.y);
            return new Vector2[] { origin, Vector2.down };
        }
        else
        {
            Vector2 origin = new Vector2(transform.position.x, bounds.max.y);
            return new Vector2[] { origin, Vector2.up };
        }
    }
}
