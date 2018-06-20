using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public GameObject target;

    MoveTrait m_MoveTrait;
    FireTrait m_FireTrait;
    FaceTarget m_FaceTarget;
    PlayerController m_PlayerController;

    // Use this for initialization
    void Start () {
        m_MoveTrait = target.GetComponent<MoveTrait>();
        m_FaceTarget = target.GetComponent<FaceTarget>();
        m_FireTrait = target.GetComponentInChildren<FireTrait>();
        m_PlayerController = target.GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (m_FaceTarget)
        {
            Vector3 inputWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_FaceTarget.SetDirection(inputWorldPosition);
        }

        if (m_MoveTrait)
        {
            float playerSpeed = m_PlayerController.maxSpeed;
            m_MoveTrait.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), playerSpeed);
        }

        if (m_FireTrait && Input.GetButton("Fire"))
            m_FireTrait.Shoot(true);
	}
}
