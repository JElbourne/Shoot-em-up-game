using UnityEngine;

public class MimicParentOpacity : MonoBehaviour {

    public SpriteRenderer m_ParentRenderer;
    SpriteRenderer m_SpriteRenderer;

    // Use this for initialization
    void Awake () {
        //m_ParentRenderer = GetComponentInParent<SpriteRenderer>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        Color m_Color = m_SpriteRenderer.color;
        m_Color.a = m_ParentRenderer.color.a;
        //Debug.Log("Color Alpha: " + m_Color.a);
        m_SpriteRenderer.color = m_Color;
    }
}
