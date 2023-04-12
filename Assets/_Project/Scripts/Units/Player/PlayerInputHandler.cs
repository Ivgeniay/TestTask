using Engine;
using Mirror;
using Units.Components;
using UnityEngine;

namespace Units.Player
{
    [RequireComponent(typeof(ChargeComponent))]
    internal class PlayerInputHandler : NetworkBehaviour
    {
        private IControlable controlable;

        #region Mono
        private void Awake() {
            controlable = GetComponent<IControlable>();
        }

        private void Update() {
            if (!isLocalPlayer) return;
            
            controlable.Move(InputActionsSystem.Instance.GetMoveInput());
        }

        private void LateUpdate() {
            if (!isLocalPlayer) return;
            
            controlable.ViewRotate(InputActionsSystem.Instance.GetMouseInput());
        }
        #endregion

        
    }
}
