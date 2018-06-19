using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrait : MonoBehaviour {

    public GameObject bullet;
    public float fireRate = 0.25f;

    float m_CooldownTimer;

    private void Start()
    {
        m_CooldownTimer = fireRate;
    }

    private void Update()
    {
        m_CooldownTimer -= Time.deltaTime;
    }

    public void Shoot(bool positive)
    {
        if (bullet && m_CooldownTimer <= 0)
        {
            Instantiate(bullet, transform.position, transform.rotation);
            m_CooldownTimer = fireRate;
        }

    }
}
