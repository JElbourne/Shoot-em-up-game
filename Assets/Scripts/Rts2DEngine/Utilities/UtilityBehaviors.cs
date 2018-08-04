using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UtilityBehaviors : MonoBehaviour {
	void Update () {
		if (Input.GetKeyDown("r")){//reload scene, for testing purposes
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

        if (Input.GetKeyDown("m"))
        {//reload scene, for testing purposes
            FindObjectOfType<LevelManager>().ChangeLevel(2);
        }

        if (Input.GetKeyDown("n"))
        {//reload scene, for testing purposes
            FindObjectOfType<LevelManager>().WinGame();
        }

    }
}
