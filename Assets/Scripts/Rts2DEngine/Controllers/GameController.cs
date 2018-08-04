using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Rts2DEngine.Map;
using Rts2DEngine.FOV;
using Rts2DEngine.UI;
using Rts2DEngine.Camera;

namespace Rts2DEngine
{
    public class GameController : MonoBehaviour
    {

        #region Singleton
        public static GameController instance;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("More than one instance of GameController Found!");
                return;
            }
            instance = this;

            m_World = FindObjectOfType<MapInstance>();
            m_UIController = FindObjectOfType<UIController>();
            m_InputController = FindObjectOfType<InputController>();
        }
        #endregion

        public List<GameObject> entities = new List<GameObject>();
        public bool lightEntireMap = false;
        public float restartDelay = 1.0f;
        public GameObject player;
        public GameObject cameraHolder;
        public GameObject miniMap;

        UIController m_UIController;
        InputController m_InputController;
        MapInstance m_World;
        GameObject m_MiniMapGo;

        public enum gameState
        {
            menu,
            playing,
            gameover
        }

        [HideInInspector]
        public gameState currentGameState = gameState.menu;

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
            if (!lightEntireMap && currentGameState == gameState.playing)
            {
                m_World.GetComponent<FieldOfViewGenerator>().GenerateFOV();
            }
        }

        public void StartGame()
        {
            // Add intial prefabs to the game.
            AddMinimap();
            //AddPlayer();

            // Set the current Game State
            currentGameState = gameState.playing;

            LevelManager.instance.StartGame(1);
        }

        public void EndGame()
        {
            if (currentGameState != gameState.gameover)
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

        private void AddMinimap()
        {
            m_MiniMapGo = Instantiate(miniMap, Vector3.zero, Quaternion.identity, cameraHolder.transform);
        }

        private void AddPlayer()
        {
            // Instantiate the player prefab
            GameObject m_PlayerGo = Instantiate(player, Vector3.zero, Quaternion.identity);

            // Setup the input controller for the player as a target
            m_InputController.SetupInput(m_PlayerGo);

            // Setup targets on the cameras
            cameraHolder.GetComponent<FollowUnit>().target = m_PlayerGo.transform;
            m_MiniMapGo.GetComponent<FollowUnit>().target = m_PlayerGo.transform;

            // Add player to the entities list
            entities.Add(m_PlayerGo);
        }
    }
}
