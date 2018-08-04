using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

    new public string name = "New Item";
    public Sprite icon = null;
    public Sprite sprite = null;

    public bool isDefaultItem = false;

    public float spawnDelay = 5.0f;
    public int maxSpawns = 1; // Used for spawners to determine how many items are in a spawner.

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
