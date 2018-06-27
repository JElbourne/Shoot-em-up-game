using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFastController : MonoBehaviour {

    MoveTrait m_MoveTrait;
    FaceTarget m_FaceTarget;
    Transform m_Target;

    public float maxSpeed = 20f;

	// Use this for initialization
	void Start () {
        m_MoveTrait = GetComponent<MoveTrait>();
        m_FaceTarget = GetComponent<FaceTarget>();
        m_Target = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_FaceTarget)
            m_FaceTarget.SetDirection(m_Target.position);

        if (m_MoveTrait)
            m_MoveTrait.Move(0, 1, maxSpeed, true);
	}
}
