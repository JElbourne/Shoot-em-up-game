using UnityEngine;

public class PlayerStats : CharacterStats {

    private void Start()
    {
        EquipmentController.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equipment _newItem, Equipment _oldItem)
    {
        if(_newItem != null)
        {
            armor.AddModifier(_newItem.armorModifier);
            damage.AddModifier(_newItem.damageModifier);
        }

        if (_oldItem != null)
        {
            armor.RemoveModifier(_oldItem.armorModifier);
            damage.RemoveModifier(_oldItem.damageModifier);
        }

    }
}
