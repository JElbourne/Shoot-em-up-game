using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item {

    public EquipmentSlotType equipmentSlotType;
    public int armorModifier;
    public int damageModifier;
    public int lightLevelModifier;
    public int speedModifier;
    public int powerModifier;

    public override void Use()
    {
        // Equip the item
        // Debug.Log("Use Equipment");
        EquipmentController.instance.Equip(this);
        // Remove from the inventory
        RemoveFromInvetory();
    }

}

public enum EquipmentSlotType
{
    Head, Chest, Legs, Weapon, Shield, Feet
}
