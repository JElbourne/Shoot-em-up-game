using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBasicController : MonoBehaviour {

    MoveTrait m_MoveTrait;
    public float maxSpeed = 10f;
    public float damage = 1f;

	// Use this for initialization
	void Start () {
        m_MoveTrait = GetComponent<MoveTrait>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_MoveTrait)
            m_MoveTrait.Move(0, 1f, maxSpeed, true);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered");
        KillableTrait killable = collision.gameObject.GetComponent<KillableTrait>();

        if (killable)
            killable.Damage(damage);
    }
}
