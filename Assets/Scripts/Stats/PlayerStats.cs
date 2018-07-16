using UnityEngine;

public class PlayerStats : CharacterStats {

    #region Singleton
    public static PlayerStats instance;

    new void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Player Stats Found!");
            return;
        }
        instance = this;
    }
    #endregion

    public IntReference maxHunger;
    public IntReference currentHunger;

    public Stat maxPower;
    public IntReference currentPower;

    public delegate void OnStatsChanged();
    public OnStatsChanged onStatsChanged;

    private void Start()
    {
        currentPower.Value = maxPower.value;
        currentHunger.Value = maxHunger.Value;

        EquipmentController.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    public new void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    public void UsePower(int _power)
    {
        int tempPower = currentPower.Value;
        tempPower -= _power;

        if (tempPower < 0)
        {
            ConsoleUI.instance.UpdateConsoleUI("Your power cells are now drained and you are stranded.");
            Die();
        }

        currentPower.Value = tempPower;
    }

    public void AddPower(int _power)
    {
        int tempPower = currentPower.Value;
        tempPower += _power;
        currentPower.Value = Mathf.Clamp(tempPower, 0, maxPower.value);
    }

    public void UseHunger(int _amount)
    {
        int tempHunger = currentHunger.Value;
        tempHunger -= _amount;

        if (tempHunger < 0)
        {
            Die();
        }

        currentHunger.Value = tempHunger;
    }

    public void AddFood(int _amount)
    {
        int tempHunger = currentHunger.Value;
        tempHunger += _amount;
        currentHunger.Value = Mathf.Clamp(tempHunger, 0, maxHunger.Value);
    }

    void OnEquipmentChanged(Equipment _newItem, Equipment _oldItem)
    {
        if(_newItem != null)
        {
            maxArmour.AddModifier(_newItem.armorModifier);
            damage.AddModifier(_newItem.damageModifier);
            lightLevel.AddModifier(_newItem.lightLevelModifier);
            maxPower.AddModifier(_newItem.powerModifier);
            maxSpeed.AddModifier(_newItem.speedModifier);
        }

        if (_oldItem != null)
        {
            maxArmour.RemoveModifier(_oldItem.armorModifier);
            damage.RemoveModifier(_oldItem.damageModifier);
            lightLevel.RemoveModifier(_oldItem.lightLevelModifier);
            maxPower.RemoveModifier(_oldItem.powerModifier);
            maxSpeed.RemoveModifier(_oldItem.speedModifier);
        }
    }

    public override void Die()
    {
        GameController.instance.EndGame();
    }
}
