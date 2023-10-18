using UnityEngine;

namespace CodeBase.PlayerBehaviour
{
    [RequireComponent(typeof(CharacterController))]
    internal class Controller : MonoBehaviour
    {
        [Header("Rotation")]
        [SerializeField] private float MinYaw = -360;
        [SerializeField] private float MaxYaw = 360;
        [SerializeField] private float MinPitch = -60;
        [SerializeField] private float MaxPitch = 60;
        [SerializeField] private float LookSensitivity = 1;

        [Space(10)]
        [Header("Move")]
        [SerializeField] private float MoveSpeed = 10;
        [SerializeField] private float SprintSpeed = 30;

        private CharacterController characterController;
        
        private Vector3 velocity;
        private float currMoveSpeed = 0;
        private float yaw;
        private float pitch;

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            ToggleControl(true);   
        }

        private void Update()
        {
            Vector3 direction = Vector3.zero;
            direction += transform.forward * Input.GetAxisRaw("Vertical");
            direction += transform.right * Input.GetAxisRaw("Horizontal");
            direction = new Vector3(direction.x, 0, direction.z);

            direction.Normalize();

            if (characterController.isGrounded)
                velocity = Vector3.zero;
            else
                velocity += -transform.up * (Mathf.Abs(Physics.gravity.y) * 10) * Time.deltaTime;
            

            if (Input.GetKey(KeyCode.LeftShift))
                currMoveSpeed = SprintSpeed;
            else
                currMoveSpeed = MoveSpeed;
            

            direction += velocity * Time.deltaTime;
            characterController.Move(direction * Time.deltaTime * currMoveSpeed);

            yaw += Input.GetAxisRaw("Mouse X") * LookSensitivity;
            pitch -= Input.GetAxisRaw("Mouse Y") * LookSensitivity;

            yaw = ClampAngle(yaw, MinYaw, MaxYaw);
            pitch = ClampAngle(pitch, MinPitch, MaxPitch);

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }


        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;

            return Mathf.Clamp(angle, min, max);
        }

        private void ToggleControl(bool isLock)
        {
		    Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
		    Cursor.visible = !isLock;
        }


    }
}
