using UnityEngine;
using Rts2DEngine;

public class EntityController : MonoBehaviour {

    void Awake()
    {
        FindObjectOfType<GameController>().entities.Add(gameObject);
    }
}
