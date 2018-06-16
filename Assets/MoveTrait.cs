using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrait : MonoBehaviour {

    Rigidbody2D m_rb;

    public float speed = 20;

	void Awake () {
        m_rb = GetComponent<Rigidbody2D>();
	}

    public void Move(float hor, float ver)
    {
        if (m_rb)
            m_rb.AddForce(new Vector2(hor * speed, ver * speed));
    }
}
