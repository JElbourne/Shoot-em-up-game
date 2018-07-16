using UnityEngine;

public class InventoryUI : MonoBehaviour {

    InventoryController m_Inventory;
    InventorySlot[] m_InventorySlots;

    // Use this for initialization
    void Start () {
        m_Inventory = InventoryController.instance;
        m_Inventory.onItemChangedCallback += UpdateInventoryUI;

        m_InventorySlots = GetComponentsInChildren<InventorySlot>();
    }
	
    void UpdateInventoryUI()
    {
        
        // Debug.Log("Updating UI, Inventory Slots Length: " + m_InventorySlots.Length);
        for (int i = 0; i < m_InventorySlots.Length; i++)
        {
            if (i < m_Inventory.items.Count)
            {
                // Debug.Log("Going to add this from UpdateUI: " + m_Inventory.items[i]);
                m_InventorySlots[i].AddItem(m_Inventory.items[i]);
            } else
            {
                m_InventorySlots[i].ClearSlot();
            }
        }

    }
}
