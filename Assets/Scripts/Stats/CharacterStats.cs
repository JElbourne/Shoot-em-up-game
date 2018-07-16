using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public IntReference maxHealth;
    public IntReference currentHealth;

    public Stat maxArmour;
    public IntReference currentArmour;

    public Stat damage;
    public Stat intelligence;

    public Stat lightLevel;
    public bool limitedLighting = false;

    public Stat maxSpeed;
    public float accelerationTime = .1f;

    protected void Awake()
    {
        currentHealth.Value = maxHealth.Value;
        // Debug.Log("currentHealth: " + currentHealth);
        if (damage.value == 0)
            Debug.LogWarning("Damage value of " + gameObject.name + " is not setup.");
        if (maxArmour.value == 0)
            Debug.LogWarning("Armour value of " + gameObject.name + " is not setup.");
        if (lightLevel.value == 0)
            Debug.LogWarning("Light Level of " + gameObject.name + " is not set so map will not light up.");
        if (maxSpeed.value == 0)
            Debug.LogWarning("Max Speed of " + gameObject.name + " is not set so movement will not work.");
    }

    public void RegenArmour(int _amount)
    {
        int tempArmour = currentArmour.Value;
        tempArmour += _amount;
        currentArmour.Value = Mathf.Clamp(tempArmour, 0, maxArmour.value);
    }

    public void AddIntelligence(int _amount)
    {
        intelligence.AddModifier(_amount);
    }

    public void RemoveIntelligence(int _amount)
    {
        intelligence.RemoveModifier(_amount);
    }

    public void TakeDamage(int _damage)
    {
        _damage = Mathf.Clamp(_damage, 0, int.MaxValue);

        int m_RemainingDamage = UseArmour(_damage);

        currentHealth.Value -= m_RemainingDamage;
        Debug.Log(transform.name + " takes " + m_RemainingDamage + " damage.");

        if (currentHealth.Value <= 0)
        {
            Die();
        }
    }

    public int UseArmour(int _damage)
    {
        int m_TempArmour = currentArmour.Value - _damage;
        if (m_TempArmour < 0)
        {
            currentArmour.Value = 0;
            return (_damage - currentArmour.Value);
        }

        currentArmour.Value = m_TempArmour;
        return 0;
    }

    public virtual void Die()
    {
        // Die in some way
        // This method is meant to be overwritten
        Debug.Log(transform.name + " died.");
    }
}
