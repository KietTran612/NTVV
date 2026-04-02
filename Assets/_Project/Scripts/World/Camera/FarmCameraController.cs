namespace NTVV.World.Camera
{
    using UnityEngine;

    /// <summary>
    /// Controller for 2D/Isometric farm camera navigation.
    /// Supports direct dragging and orthographic zooming.
    /// </summary>
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class FarmCameraController : MonoBehaviour
    {
        [Header("Pan Settings")]
        [SerializeField] private float _panSpeed = 1.0f;
        [SerializeField] private Vector2 _boundX = new Vector2(-10, 10);
        [SerializeField] private Vector2 _boundY = new Vector2(-10, 10);

        [Header("Zoom Settings")]
        [SerializeField] private float _zoomSpeed = 4f;
        [SerializeField] private float _minOrtho = 3f;
        [SerializeField] private float _maxOrtho = 10f;

        private UnityEngine.Camera _cam;
        private Vector3 _lastMousePos;
        private bool _isDragging;

        private void Awake()
        {
            _cam = GetComponent<UnityEngine.Camera>();
        }

        private void Update()
        {
            HandlePan();
            HandleZoom();
        }

        /// <summary>
        /// Direct drag logic by calculating mouse delta in world/screen space.
        /// </summary>
        private void HandlePan()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastMousePos = Input.mousePosition;
                _isDragging = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
            }

            if (_isDragging && Input.GetMouseButton(0))
            {
                // Calculate movement delta since last frame
                Vector3 delta = Input.mousePosition - _lastMousePos;
                
                // Convert screen delta to proportional world movement 
                // Using orthoSize to keep movement speed consistent with zoom level
                float screenFactor = _cam.orthographicSize / Screen.height * 2.0f;
                Vector3 move = new Vector3(-delta.x * screenFactor * _panSpeed, -delta.y * screenFactor * _panSpeed, 0);

                Vector3 targetPos = transform.position + move;

                // Clamp position within bounds
                float clampedX = Mathf.Clamp(targetPos.x, _boundX.x, _boundX.y);
                float clampedY = Mathf.Clamp(targetPos.y, _boundY.x, _boundY.y);
                
                transform.position = new Vector3(clampedX, clampedY, transform.position.z);
                
                _lastMousePos = Input.mousePosition;
            }
        }

        /// <summary>
        /// Zoom via orthographic size adjustment.
        /// </summary>
        private void HandleZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                float targetOrtho = _cam.orthographicSize - (scroll * _zoomSpeed);
                _cam.orthographicSize = Mathf.Clamp(targetOrtho, _minOrtho, _maxOrtho);
                
                // After zoom, re-clamp position in case current position is now outside based on view
                // (Optional: add logic here if bounds depend on ortho size)
            }
        }

        /// <summary>
        /// Dynamic bounds adjustment if needed.
        /// </summary>
        public void SetBounds(Vector2 xRange, Vector2 yRange)
        {
            _boundX = xRange;
            _boundY = yRange;
        }
    }
}
