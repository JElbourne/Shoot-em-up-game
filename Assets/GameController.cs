using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {

    public float restartDelay = 1.0f;
    public GameObject gameOverScreen;

    bool m_GameHasEnded = false;

    public void EndGame()
    {
        if(m_GameHasEnded == false)
        {
            m_GameHasEnded = true;
            gameOverScreen.SetActive(true);
            Invoke("RestartScene", restartDelay);
        }
        
    }

    private void RestartScene()
    {
        gameOverScreen.SetActive(false);
        m_GameHasEnded = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
