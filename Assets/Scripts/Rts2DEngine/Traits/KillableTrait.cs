using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class KillableTrait : MonoBehaviour {

    CharacterStats m_Stats;

    private void Awake()
    {
        m_Stats = GetComponent<CharacterStats>();
        if(m_Stats == null)
        {
            Debug.Log("Character Stats not found on Game Object " + gameObject.name);
        }
    }

    public void Damage(int damage)
    {
        if(m_Stats)
            m_Stats.TakeDamage(damage);
    }
}
