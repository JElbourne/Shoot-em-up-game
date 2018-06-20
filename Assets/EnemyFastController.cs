using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFastController : MonoBehaviour {

    MoveTrait m_MoveTrait;
    FaceTarget m_FaceTarget;

    public float maxSpeed = 20f;
    public Transform target;

	// Use this for initialization
	void Start () {
        m_MoveTrait = GetComponent<MoveTrait>();
        m_FaceTarget = GetComponent<FaceTarget>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_FaceTarget)
            m_FaceTarget.SetDirection(target.position);

        if (m_MoveTrait)
            m_MoveTrait.Move(0, 1, maxSpeed, true);
	}
}
