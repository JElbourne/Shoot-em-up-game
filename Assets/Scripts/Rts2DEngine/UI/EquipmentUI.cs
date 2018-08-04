using UnityEngine;

public class EquipmentUI : MonoBehaviour {

    EquipmentSlot[] m_EquipmentSlots;

	void Start () {
        EquipmentController.instance.onEquipmentChanged += UpdateEquipmentUI;

        m_EquipmentSlots = GetComponentsInChildren<EquipmentSlot>();
	}
	
    void UpdateEquipmentUI(Equipment _newItem, Equipment _oldItem)
    {
        if (_newItem != null)
        {
            int m_Index = (int)_newItem.equipmentSlotType;
            m_EquipmentSlots[m_Index].AddEquipment(_newItem);
            // Debug.Log("Adding Equipment " + _newItem.name + " to slot at index: " + m_Index);
        }

        if (_oldItem != null)
        {
            int m_Index = (int)_oldItem.equipmentSlotType;
            m_EquipmentSlots[m_Index].ClearSlot();
            // Debug.Log("Removing Equipment " + _oldItem.name + " to slot at index: " + m_Index);
        }
    }
}
