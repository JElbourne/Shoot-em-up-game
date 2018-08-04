using UnityEngine;

namespace Rts2DEngine
{
    namespace Camera
    {
        public class FollowUnit : MonoBehaviour
        {

            [HideInInspector]
            public Transform target;
            public float smoothSpeed = 10f; // High value the faster is locks on to target.
            public Vector3 offset;

            GameController m_GameController;

            private void Awake()
            {
                m_GameController = FindObjectOfType<GameController>();
            }

            void LateUpdate()
            {
                if (m_GameController.currentGameState == GameController.gameState.playing &&
                    target != null)
                {
                    Vector3 desiredPosition = target.position + offset;
                    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
                    transform.position = smoothedPosition;
                }
            }
        }
    }
}