using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrait : MonoBehaviour {

    public GameObject bullet;
    public float fireRate = 0.25f;

    public CameraShakeTrait cameraShakeTrait;
    public float cameraShakeDuration = .05f;
    public float cameraShakeMagnitude = .1f;

    float m_CooldownTimer;
    AudioSource m_AudioSource;

    private void Start()
    {
        m_CooldownTimer = fireRate;
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        m_CooldownTimer -= Time.deltaTime;
    }

    public void Shoot(bool positive)
    {
        if (bullet && m_CooldownTimer <= 0)
        {
            Instantiate(bullet, transform.position, transform.parent.rotation);
            m_AudioSource.Play();
            StartCoroutine(cameraShakeTrait.Shake(cameraShakeDuration, cameraShakeMagnitude));

            m_CooldownTimer = fireRate;
        }

    }
}
