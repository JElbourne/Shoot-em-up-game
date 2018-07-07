using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public GameObject target;

    MoveTrait m_MoveTrait;
    FireTrait m_FireTrait;
    FaceTargetTrait m_FaceTargetTrait;
    InteractTrait m_InteractTrait;
    PlayerController m_PlayerController;

    // Use this for initialization
    void Start () {
        m_MoveTrait = target.GetComponent<MoveTrait>();
        m_FaceTargetTrait = target.GetComponent<FaceTargetTrait>();
        m_FireTrait = target.GetComponentInChildren<FireTrait>();
        m_InteractTrait = target.GetComponentInChildren<InteractTrait>();
        m_PlayerController = target.GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (m_FaceTargetTrait)
        {
            Vector3 m_MousePos = Input.mousePosition;
            Vector3 inputWorldPosition = Camera.main.ScreenToWorldPoint(m_MousePos);
            m_FaceTargetTrait.SetDirection(inputWorldPosition);
        }

        if (m_MoveTrait)
        {
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            m_MoveTrait.Move(input);
        }

        if (m_FireTrait && Input.GetButton("Fire"))
            m_FireTrait.Shoot(true);

        if (m_InteractTrait && Input.GetButton("Interact"))
        {
            m_InteractTrait.Interact();
        }
	}
}
