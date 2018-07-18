public class ItemSpawnController : ItemController {

    int m_UsedCount = 0;
    bool m_SpawnerActive = true;

    public new void Action()
    {
        if (!m_SpawnerActive)
            return;

        bool itemAdded = InventoryController.instance.Add(item);

        if (itemAdded)
        {
            m_SpriteRenderer.sprite = null;
            m_SpawnerActive = false;
            m_UsedCount++;
            if (m_UsedCount < item.maxSpawns)
            {
                Invoke("ResetSpawner", item.spawnDelay);
            }
        }
    }

    void ResetSpawner()
    {
        m_SpriteRenderer.sprite = item.sprite;
        m_SpawnerActive = true;
    }
}
