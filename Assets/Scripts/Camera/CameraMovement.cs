using UnityEngine;

namespace Core.UI
{
    public class CameraMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float edgeThickness = 100f; // Ширина активирования движения от краёв экрана
        [SerializeField] private Vector3 minBounds; // Минимальная и максимальная граница движения
        [SerializeField] private Vector3 maxBounds;

        [Header("Zoom Settings")]
        [SerializeField] private float zoomSpeed = 2f;
        [SerializeField] private float minZoom = 5f;
        [SerializeField] private float maxZoom = 20f;

        [SerializeField] private Camera cam;

        private void Update()
        {
            HandleMovement();
            HandleZoom();
        }

        private void HandleMovement()
        {
            Vector3 movement = new Vector3(
                (Input.mousePosition.x > Screen.width - edgeThickness ? 1 : 0) - (Input.mousePosition.x < edgeThickness ? 1 : 0),
                0,
                (Input.mousePosition.y > Screen.height - edgeThickness ? 1 : 0) - (Input.mousePosition.y < edgeThickness ? 1 : 0)
            );

            movement.Normalize();

            Vector3 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;

            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            newPosition.z = Mathf.Clamp(newPosition.z, minBounds.z, maxBounds.z);

            transform.position = newPosition;
        }


        private void HandleZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (cam.orthographic == false)
            {
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - scroll * zoomSpeed, minZoom, maxZoom);
            }
            else
            {
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube((minBounds + maxBounds) / 2, maxBounds - minBounds);
        }
    }
}