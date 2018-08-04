using UnityEngine;
using Rts2DEngine.Map;

namespace Rts2DEngine
{
    namespace Camera
    {
        public class FreeFlow : MonoBehaviour
        {
            // TODO
            // Smoothing
            // Pan while click mouse wheel
            // Unit following
            // Keyboard smoothing

            public float panSpeed = 20f;
            public float scrollSpeed = 20f;
            public float panBorderThickness = 10f;
            public int minCameraOrtho = 5;
            public int maxCameraOrtho = 8;

            UnityEngine.Camera m_MainCamera;
            Vector2[] panLimits;

            private void Start()
            {
                m_MainCamera = UnityEngine.Camera.main;
                panLimits = MapInstance.instance.GetWorldPanLimit();
            }

            private void Update()
            {
                Vector3 pos = transform.position;

                if (Input.GetKey(KeyCode.UpArrow) || (Input.mousePosition.y >= Screen.height - panBorderThickness && Input.mousePosition.y < Screen.height - 1))
                {
                    pos.y += panSpeed * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.DownArrow) || (Input.mousePosition.y <= panBorderThickness && Input.mousePosition.y > 1))
                {
                    pos.y -= panSpeed * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.RightArrow) || (Input.mousePosition.x >= Screen.width - panBorderThickness && Input.mousePosition.x < Screen.width - 1))
                {
                    pos.x += panSpeed * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.LeftArrow) || (Input.mousePosition.x <= panBorderThickness && Input.mousePosition.x > 1))
                {
                    pos.x -= panSpeed * Time.deltaTime;
                }

                float newCameraOrtho = m_MainCamera.orthographicSize - (Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * 100f * Time.deltaTime);
                m_MainCamera.orthographicSize = Mathf.Clamp(newCameraOrtho, minCameraOrtho, maxCameraOrtho);

                pos.x = Mathf.Clamp(pos.x, panLimits[0].x, panLimits[1].x);
                pos.y = Mathf.Clamp(pos.y, panLimits[0].y, panLimits[1].y);

                transform.position = pos;
            }
        }
    }
}
    
