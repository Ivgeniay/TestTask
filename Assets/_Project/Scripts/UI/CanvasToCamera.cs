using UnityEngine;

namespace UI
{
    internal class CanvasToCamera : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        private Camera playerCamera;

        private void Awake() {
            if (!canvas) canvas = GetComponent<Canvas>();
            playerCamera = Camera.main;
        }

        private void Update() {
            canvas.transform.LookAt(playerCamera.transform);
        }
    }
}
