using UnityEngine;
using Rts2DEngine.Map;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemController : MonoBehaviour {

    public Item item;

    protected SpriteRenderer m_SpriteRenderer;

    public void Awake()
    {
        if(item)
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sprite = item.sprite;
        }
    }
    // Called by an Interactable Trait
    public void Action()
    {
        // Debug.Log("PickedUp: " + item.name);
        bool itemAdded = InventoryController.instance.Add(item);
        if (itemAdded)
        {
            MapInstance.instance.RemoveFromItemsData(transform);
            Destroy(gameObject);
        }

    }
}
