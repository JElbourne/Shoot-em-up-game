using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth { get; private set; }

    public Stat damage;
    public Stat armor;

    public Stat lightLevel;
    public bool limitedLighting = false;

    public Stat maxSpeed;
    public float accelerationTime = .1f;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        if (damage.getValue() == 0)
            Debug.LogWarning("Damage value of " + gameObject.name + " is not setup.");
        if (armor.getValue() == 0)
            Debug.LogWarning("Armour value of " + gameObject.name + " is not setup.");
        if (lightLevel.getValue() == 0)
            Debug.LogWarning("Light Level of " + gameObject.name + " is not set so map will not light up.");
        if (maxSpeed.getValue() == 0)
            Debug.LogWarning("Max Speed of " + gameObject.name + " is not set so movement will not work.");
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
