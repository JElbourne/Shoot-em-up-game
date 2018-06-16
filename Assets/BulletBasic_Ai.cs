using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBasic_Ai : MonoBehaviour {

    MoveTrait m_MoveTrait;

    float m_Direction = 0;

	// Use this for initialization
	void Start () {
        m_MoveTrait = GetComponent<MoveTrait>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_MoveTrait)
            m_MoveTrait.Move(0, m_Direction);
	}

    public void SetDirection(bool positive)
    {
        if (positive)
        {
            m_Direction = 1;
        }
        else
        {
            m_Direction = -1;
        }
            
    }
}
