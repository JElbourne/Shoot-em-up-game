using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemController : MonoBehaviour {

    public Item item;

    public void Awake()
    {
        if(item)
        {
            GetComponent<SpriteRenderer>().sprite = item.sprite;
        }
    }
    // Called by an Interactable Trait
    public void Action()
    {
        // Debug.Log("PickedUp: " + item.name);
        bool itemAdded = InventoryController.instance.Add(item);
        if (itemAdded)
        {
            WorldInstance.instance.RemoveFromItemsData(transform);
            Destroy(gameObject);
        }

    }
}
