using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {

    public List<GameObject> entities = new List<GameObject>();
    public bool lightEntireMap = false;
    public float restartDelay = 1.0f;
    public GameObject gameOverScreen;

    WorldInstance m_World;
    bool m_GameHasEnded = false;

    private void Start()
    {
        m_World = FindObjectOfType<WorldInstance>();
        m_World.Setup();
        if (lightEntireMap)
        {
            m_World.LightWorld();
        }
    }

    private void LateUpdate()
    {
        if (!lightEntireMap)
        {
            m_World.GetComponent<FieldOfViewGenerator>().GenerateFOV();
        }
    }

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
