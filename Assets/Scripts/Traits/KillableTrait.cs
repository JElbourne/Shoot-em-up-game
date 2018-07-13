using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillableTrait : MonoBehaviour {

    PlayerStats m_PlayerStats;

    private void Awake()
    {
        m_PlayerStats = GetComponent<PlayerStats>();
    }

    public void Damage(int damage)
    {
        m_PlayerStats.TakeDamage(damage);
    }
}
