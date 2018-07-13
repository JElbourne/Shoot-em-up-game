using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth { get; private set; }

    public Stat damage;
    public Stat armor;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int _damage)
    {

        _damage -= armor.getValue();
        _damage = Mathf.Clamp(_damage, 0, int.MaxValue);

        currentHealth -= _damage;
        Debug.Log(transform.name + " takes " + _damage + " damage.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // Die in some way
        // This method is meant to be overwritten
        Debug.Log(transform.name + " died.");
    }
}
