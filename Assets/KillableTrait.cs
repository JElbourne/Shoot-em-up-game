using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillableTrait : MonoBehaviour {

    public float health = 20f;

    public void Damage(float damage)
    {
        health -= damage;
        // Debug.Log("Current Health: " + health);
        if (health <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
