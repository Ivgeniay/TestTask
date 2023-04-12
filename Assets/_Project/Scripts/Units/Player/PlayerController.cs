using UnityEngine;
using Mirror;
using System.Runtime.CompilerServices;
using Unitils;
using System.Linq;

namespace Units.Player
{
    [RequireComponent(typeof(Rigidbody))]
    internal class PlayerController : NetworkBehaviour, IControlable
    {
        [SyncVar][SerializeField] private float speed;
        [SerializeField] private Transform modelTransform;
        

        [Header("CameraSettings")]
        [SerializeField] private Vector3 cameraOffset = new Vector3(0, 1.75f, -3.5f);
        [SerializeField] private float yMaxAngle = 40;
        [SerializeField] private float yMinAngle = -20;
        [SerializeField] private float xMouseSens = 2.5f;
        [SerializeField] private float yMouseSens = 2f;
        [SerializeField] private bool isXInverse = false;
        [SerializeField] private bool isYInverse = false;

        private Camera followCamera;
        private Transform cameraPointer;
        private Rigidbody rb;
        private Vector3 moveInput;
        private float mouseX;
        private float mouseY;

        #region Mono
        private void Awake() {
            rb = GetComponent<Rigidbody>();
        }

        private void Start() {
            if (isLocalPlayer) {
                followCamera = Camera.main;

                cameraPointer = new GameObject("CameraPointer").transform;
                followCamera.transform.parent = cameraPointer;
                followCamera.transform.localPosition = cameraOffset;
            }
        }

        private void FixedUpdate() {
            rb.MovePosition(rb.position + moveInput * speed * Time.deltaTime);
        }
        #endregion

        public void Move(Vector2 moveDirection) {
            if (!isLocalPlayer) return;
            cameraPointer.position = transform.position;

            moveDirection = moveDirection.normalized;

            moveInput = new Vector3(moveDirection.x, 0, moveDirection.y);
            moveInput = followCamera!.transform.forward * moveInput.z + followCamera!.transform.right * moveInput.x;
            moveInput = new Vector3(moveInput.x, 0, moveInput.z);
        }

        public void ViewRotate(Vector2 moveDelta) {
            if (isXInverse) mouseX -= (moveDelta.x * xMouseSens);
            else mouseX += (moveDelta.x * xMouseSens);
            if (isYInverse) mouseY += (moveDelta.y * yMouseSens);
            else mouseY -= (moveDelta.y * yMouseSens);

            RotateBody();

            mouseY = Mathf.Clamp(mouseY, yMinAngle, yMaxAngle);
            cameraPointer.localRotation = Quaternion.Euler(mouseY, mouseX, 0);
        }

        private void RotateBody() {
            if (moveInput != Vector3.zero) 
                transform.localRotation = Quaternion.Euler(0, mouseX, 0);
        }

    }
}
