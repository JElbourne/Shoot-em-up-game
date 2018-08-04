using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour {

    public Image icon;
    public Button removeButton;

    Equipment m_Equipment;

    public void AddEquipment(Equipment _newItem)
    {
        // Debug.Log("Going to add this from AddEquipment: " + _newItem);
        m_Equipment = _newItem;

        icon.sprite = _newItem.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        m_Equipment = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        EquipmentController.instance.Unequip((int)m_Equipment.equipmentSlotType);
    }
}
