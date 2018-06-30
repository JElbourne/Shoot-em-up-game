using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public GameObject target;

    MoveTrait m_MoveTrait;
    FireTrait m_FireTrait;
    FaceTargetTrait m_FaceTargetTrait;
    PlayerController m_PlayerController;

    // Use this for initialization
    void Start () {
        m_MoveTrait = target.GetComponent<MoveTrait>();
        m_FaceTargetTrait = target.GetComponent<FaceTargetTrait>();
        m_FireTrait = target.GetComponentInChildren<FireTrait>();
        m_PlayerController = target.GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (m_FaceTargetTrait)
        {
            Vector3 m_MousePos = Input.mousePosition;
            //m_MousePos.z = 10;
            Vector3 inputWorldPosition = Camera.main.ScreenToWorldPoint(m_MousePos);
            m_FaceTargetTrait.SetDirection(inputWorldPosition);
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
