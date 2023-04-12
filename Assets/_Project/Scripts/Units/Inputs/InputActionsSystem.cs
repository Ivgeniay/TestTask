using System;
using UnityEngine;

public class InputActionsSystem : MonoBehaviour
{
    public event Action OnChargePerformedEvent; 

    private Controls inputActions;

    #region Singleton
    private static InputActionsSystem instance;
    public static InputActionsSystem Instance {
        get {
            if (instance == null) {
                var go = new GameObject("[INPUTSYSTEM]");
                instance = go.AddComponent<InputActionsSystem>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }
    #endregion
    #region Mono
    private void Awake() {
        inputActions = new Controls();
    }

    private void OnEnable() {
        inputActions.Enable();
        inputActions.FPS.Charge.performed += OnChargePerformedHandler;
    }

    private void OnDisable() {
        inputActions.FPS.Charge.performed -= OnChargePerformedHandler;
        inputActions.Disable();
    }
    #endregion

    public Vector2 GetMoveInput() =>
        inputActions.FPS.WASD.ReadValue<Vector2>();

    public Vector2 GetMouseInput() =>
        inputActions.FPS.MouseMove.ReadValue<Vector2>();
    
    private void OnChargePerformedHandler(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnChargePerformedEvent?.Invoke();
    }
    
}
