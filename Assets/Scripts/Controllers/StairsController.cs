using UnityEngine;

public class StairsController : MonoBehaviour {
    public int toLevel;

    // Called by an Interactable Trait
    public void Action () {
        LevelManager.instance.ChangeLevel(toLevel);
	}

}
