using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

    new public string name = "New Item";
    public Sprite icon = null;
    public Sprite replacementIcon = null;

    public bool isDefaultItem = false;

    public virtual void Use()
    {
        // Use the Item
        // Something might happen

        Debug.Log("Using " + name);
    }

    public void RemoveFromInvetory()
    {
        InventoryController.instance.Remove(this);
    }
}
