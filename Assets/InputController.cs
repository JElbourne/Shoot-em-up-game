using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public GameObject target;

    MoveTrait m_MoveTrait;
    FireTrait m_FireTrait;
    FaceInput m_FaceInput;
    PlayerController m_PlayerController;

    // Use this for initialization
    void Start () {
        m_MoveTrait = target.GetComponent<MoveTrait>();
        m_FaceInput = target.GetComponent<FaceInput>();
        m_FireTrait = target.GetComponentInChildren<FireTrait>();
        m_PlayerController = target.GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (m_FaceInput)
            m_FaceInput.SetDirection(Input.mousePosition);

        if (m_MoveTrait)
        {
            float playerSpeed = m_PlayerController.maxSpeed;
            m_MoveTrait.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), playerSpeed);
        }

        if (m_FireTrait && Input.GetButton("Fire"))
            m_FireTrait.Shoot(true);
	}
}
