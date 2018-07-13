using UnityEngine;

public class ItemController : MonoBehaviour {

    public Item item;

    public Sprite replacementSprite;

    // Called by an Interactable Trait
    public void Action()
    {
        Debug.Log("PickedUp: " + item.name);
        bool itemAdded = InventoryController.instance.Add(item);
        if (itemAdded)
        {
            GetComponent<SpriteRenderer>().sprite = replacementSprite;
            Destroy(GetComponent<InteractableTrait>());
        }

    }
}
