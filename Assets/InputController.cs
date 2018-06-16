using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public GameObject target;

    MoveTrait m_MoveTrait;
    FireTrait m_FireTrait;

	// Use this for initialization
	void Start () {
        m_MoveTrait = target.GetComponent<MoveTrait>();
        m_FireTrait = target.GetComponentInChildren<FireTrait>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_MoveTrait)
            m_MoveTrait.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (m_FireTrait && Input.GetButton("Fire"))
            m_FireTrait.Shoot(true);
	}
}
