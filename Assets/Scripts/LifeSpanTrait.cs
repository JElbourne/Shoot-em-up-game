using UnityEngine;

public class LifeSpanTrait : MonoBehaviour {

    public float LifeSpan = 1f;

    float m_CountdownTimer;

	// Use this for initialization
	void Start () {
        m_CountdownTimer = LifeSpan;
	}
	
	// Update is called once per frame
	void Update () {
        m_CountdownTimer -= Time.deltaTime;
        if (m_CountdownTimer <= 0)
            Die();
	}

    public void Die()
    {
        Destroy(gameObject);
    }
}
