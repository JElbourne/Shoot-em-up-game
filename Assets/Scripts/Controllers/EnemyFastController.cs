using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFastController : MonoBehaviour {

    MoveTrait m_MoveTrait;
    FaceTargetTrait m_FaceTargetTrait;
    Transform m_Target;

    public float maxSpeed = 20f;
    public float accelerationTime = 0.1f;

    // Use this for initialization
    void Start () {
        m_MoveTrait = GetComponent<MoveTrait>();
        m_FaceTargetTrait = GetComponent<FaceTargetTrait>();
        m_Target = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_FaceTargetTrait)
            m_FaceTargetTrait.SetDirection(m_Target.position);

        if (m_MoveTrait)
            m_MoveTrait.Move(maxSpeed, accelerationTime, new Vector2(0, 1), true);
	}
}
