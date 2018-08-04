using UnityEngine;
using UnityEngine.Events;

public class InteractableTrait : MonoBehaviour {

    public UnityEvent action;

    public void Interact()
    {
        // Debug.Log("Invoking Action");
        action.Invoke();
    }
}
