using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFast_Ai : MonoBehaviour {

    MoveTrait m_MoveTrait;

	// Use this for initialization
	void Start () {
        m_MoveTrait = GetComponent<MoveTrait>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_MoveTrait)
            m_MoveTrait.Move(0, -1);
	}
}
