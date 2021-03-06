﻿using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public Image icon;
    public Button removeButton;

    Item m_Item;

    public void AddItem(Item _newItem)
    {
        m_Item = _newItem;

        icon.sprite = _newItem.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        m_Item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        InventoryController.instance.Remove(m_Item);
        
    }

    public void UseItem()
    {
        // Debug.Log("Tried to use m_Item: " + m_Item);
        if (m_Item != null)
        {
            m_Item.Use();
        }
    }
}
