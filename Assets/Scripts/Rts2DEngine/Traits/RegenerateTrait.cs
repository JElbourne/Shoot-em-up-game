using UnityEngine;
using Rts2DEngine;

[RequireComponent(typeof(PlayerStats))]
public class RegenerateTrait : MonoBehaviour {

    public float regenerationRate = 10f;
    public bool costsEnergy = true;
    public int energyCost = 1;

    float m_Cooldown = 0;

	// Use this for initialization
	void Start () {
        m_Cooldown = regenerationRate;

    }
	
	// Update is called once per frame
	void Update () {
        m_Cooldown -= Time.deltaTime;

        if (m_Cooldown < 0)
        {
            PlayerStats.instance.RegenArmour(1);
            if (costsEnergy)
            {
                PlayerStats.instance.UsePower(energyCost);
            }
            m_Cooldown = regenerationRate;
        }

    }
}
