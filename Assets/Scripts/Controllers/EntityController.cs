using UnityEngine;

public class EntityController : MonoBehaviour {

    void Awake()
    {
        FindObjectOfType<GameController>().entities.Add(gameObject);
    }
}
