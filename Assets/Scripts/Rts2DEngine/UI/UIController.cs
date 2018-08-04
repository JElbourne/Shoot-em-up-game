using UnityEngine;

namespace Rts2DEngine
{
    namespace UI
    {
        public class UIController : MonoBehaviour
        {

            public GameObject gameOverScreen;
            public GameObject menuScreen;

            public void startGameClicked()
            {
                menuScreen.SetActive(false);
                FindObjectOfType<GameController>().StartGame();
            }
        }
    }
}

