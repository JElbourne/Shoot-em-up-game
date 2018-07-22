using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    
    SpriteRenderer m_SpriteRenderer;
    string m_Name;
    bool m_HasCollider;

    public GameObject tileGo { get; private set; }
    public int movementCost = 1;

    public Vector3 pos;

    public Tile(GameObject _tileGo, SpriteRenderer _spriteRenderer, string _name, bool _hasCollider = false)
    {
        tileGo = _tileGo;
        m_SpriteRenderer = _spriteRenderer;
        m_Name = _name;
        m_HasCollider = _hasCollider;
        pos = _tileGo.transform.position;
    }

    public bool isEnabled()
    {
        return m_SpriteRenderer.enabled;
    }

    public string getName()
    {
        return m_Name;
    }

    public void ToggleVisible()
    {
        if(m_SpriteRenderer.enabled)
        {
            m_SpriteRenderer.enabled = false;
        } else
        {
            m_SpriteRenderer.enabled = true;
        }
    }

    public void EnableVisible()
    {
        m_SpriteRenderer = tileGo.GetComponent<SpriteRenderer>();
        m_SpriteRenderer.enabled = true;
    }

    public void DisableVisible()
    {
        m_SpriteRenderer.enabled = false;
    }

    public void SetOpacity(float _opacity)
    {
        if(m_SpriteRenderer != null && m_SpriteRenderer.color.a != (_opacity / 100f))
        {
            m_SpriteRenderer.color = new Color(1f, 1f, 1f, (_opacity / 100f));
        }
    }

    public bool hasCollider()
    {
        return m_HasCollider;
    }

    public Vector3 getCoord()
    {
        return tileGo.transform.position;
    }
}
