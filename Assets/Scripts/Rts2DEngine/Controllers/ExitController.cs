using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour {

	// Use this for initialization
	public void Action () {
        FindObjectOfType<LevelManager>().WinGame();
    }
}
