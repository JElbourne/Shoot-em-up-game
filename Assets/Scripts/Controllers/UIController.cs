using UnityEngine;

public class UIController: MonoBehaviour {

    public GameObject gameOverScreen;
    public GameObject menuScreen;

    public void startGameClicked()
    {
        menuScreen.SetActive(false);
        FindObjectOfType<GameController>().StartGame();
    }

}
