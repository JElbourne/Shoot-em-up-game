using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour {

    #region Singleton
    public static EquipmentController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Equipment Controller Found!");
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnEquipmentChanged(Equipment _newItem, Equipment _oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    Equipment[] currentEquipment;
    InventoryController inventory;

    private void Start()
    {
        inventory = InventoryController.instance;
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
    }

    public void Equip (Equipment _newItem)
    {
        int slotIndex = (int)_newItem.equipmentSlot;

        Equipment m_OldItem = null;

        if (currentEquipment[slotIndex] != null)
        {
            m_OldItem = currentEquipment[slotIndex];
            inventory.Add(m_OldItem);
        }

        if(onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(_newItem, m_OldItem);
        }

        currentEquipment[slotIndex] = _newItem;
    }

    public void Unequip(int slotIndex)
    {
        if(currentEquipment[slotIndex] != null)
        {
            Equipment m_OldItem = currentEquipment[slotIndex];
            inventory.Add(m_OldItem);

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, m_OldItem);
            }

            currentEquipment[slotIndex] = null;
        }
    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
    }
}
