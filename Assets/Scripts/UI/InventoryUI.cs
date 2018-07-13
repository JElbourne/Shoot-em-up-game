using UnityEngine;

public class InventoryUI : MonoBehaviour {

    InventoryController m_Inventory;
    InventorySlot[] m_Slots;

	// Use this for initialization
	void Start () {
        m_Inventory = InventoryController.instance;
        m_Inventory.onItemChangedCallback += UpdateUI;

        m_Slots = GetComponentsInChildren<InventorySlot>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateUI()
    {
        // Debug.Log("Updating UI");
        for (int i = 0; i < m_Slots.Length; i++)
        {
            if (i < m_Inventory.items.Count)
            {
                Debug.Log("Going to add this from UpdateUI: " + m_Inventory.items[i]);
                m_Slots[i].AddItem(m_Inventory.items[i]);
            } else
            {
                m_Slots[i].ClearSlot();
            }
        }

    }
}
