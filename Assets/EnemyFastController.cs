using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFastController : MonoBehaviour {

    MoveTrait m_MoveTrait;
    public float maxSpeed = 20f;

	// Use this for initialization
	void Start () {
        m_MoveTrait = GetComponent<MoveTrait>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_MoveTrait)
            m_MoveTrait.Move(0, -1, maxSpeed);
	}
}
