using UnityEngine;

public class EntityController : MonoBehaviour {

    GameController m_Game;

    public float maxSpeed = 5f;
    public int lightLevel = 0;
    public int level = 1;
    public bool limitedLighting = false;

    void Awake()
    {
        FindObjectOfType<GameController>().entities.Add(gameObject);
    }

    public int[] getTileCoord()
    {
        int posX = Mathf.RoundToInt(transform.position.x);
        int posY = Mathf.RoundToInt(transform.position.y);
        return new int[2] { posX, posY };
    }
}
