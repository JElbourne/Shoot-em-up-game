using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBasicController : MonoBehaviour {
    MoveTrait m_MoveTrait;
    public float damage = 1f;
    public float maxSpeed = 50f;
    public float accelerationTime = 0.1f;

    public LayerMask collisionMask;


    // Use this for initialization
    void Start () {
        m_MoveTrait = GetComponent<MoveTrait>();
    }
	
	// Update is called once per frame
	void Update () {
        if (m_MoveTrait)
            m_MoveTrait.Move(maxSpeed, accelerationTime, new Vector2(0, 1f), true);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, .1f, collisionMask);
        if (hit)
        {
            KillableTrait killable = hit.transform.GetComponent<KillableTrait>();

            if (killable)
                killable.Damage(damage);

            Die();
        }

    }

    //private void OnColliderEnter2D(Collider2D collision)
    //{
    //    Die();
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    KillableTrait killable = collision.gameObject.GetComponent<KillableTrait>();

    //    if (killable)
    //        killable.Damage(damage);

    //    Die();
    //}

    private void Die()
    {
        FindObjectOfType<GameController>().entities.Remove(gameObject);
        Destroy(gameObject);
    }
}
