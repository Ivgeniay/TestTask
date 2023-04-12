using Mirror;
using UnityEngine.Events;

namespace Units.Player
{
    internal sealed class TakeDamageComponent : NetworkBehaviour
    {
        public UnityEvent OnTakeDamageEvent;
        public bool IsImmortal { get; private set; }

        [ClientRpc]
        public void TakeDamage() {
            if (IsImmortal) return;
            OnTakeDamageEvent?.Invoke();
        }

        public void SetImmortal(bool isImmortal) =>
            this.IsImmortal = isImmortal;
    }
}
