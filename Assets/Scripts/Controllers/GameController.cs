using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {

    public List<GameObject> entities = new List<GameObject>();
    public bool lightEntireMap = false;
    public float restartDelay = 1.0f;
    public GameObject player;
    public GameObject cameraHolder;
    public Camera miniMapCamera;

    UIController m_UIController;
    InputController m_InputController;
    WorldInstance m_World;

    public enum gameState
    {
        menu,
        pilot,
        mechanic,
        navigator,
        gameover
    }

    [HideInInspector]
    public gameState currentGameState = gameState.menu;

    private void Awake()
    {
        m_World = FindObjectOfType<WorldInstance>();
        m_UIController = FindObjectOfType<UIController>();
        m_InputController = FindObjectOfType<InputController>();
    }
    private void Start()
    {
        m_World.Setup();
        if (lightEntireMap)
        {
            m_World.LightWorld();
        }
    }

    private void LateUpdate()
    {
        if (!lightEntireMap && currentGameState == gameState.pilot)
        {
            m_World.GetComponent<FieldOfViewGenerator>().GenerateFOV();
        }
    }

    public void StartGame()
    {
        GameObject m_PlayerGo = Instantiate(player, Vector3.zero, Quaternion.identity);
        cameraHolder.GetComponent<CameraFollow>().target = m_PlayerGo.transform;
        miniMapCamera.GetComponent<CameraFollow>().target = m_PlayerGo.transform;
        m_InputController.SetupInput(m_PlayerGo);
        currentGameState = gameState.pilot;
    }

    public void EndGame()
    {
        if(currentGameState != gameState.gameover)
        {
            currentGameState = gameState.gameover;
            m_UIController.gameOverScreen.SetActive(true);
            Invoke("RestartScene", restartDelay);
        }
        
    }

    private void RestartScene()
    {
        m_UIController.gameOverScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
