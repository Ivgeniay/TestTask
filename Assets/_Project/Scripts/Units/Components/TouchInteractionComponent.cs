using Mirror;
using TMPro;
using UnityEngine;

namespace Units.Components
{
    [RequireComponent(typeof(TouchCounterComponent))]
    internal class TouchInteractionComponent : NetworkBehaviour
    {
        [SerializeField] private TouchCounterComponent touchCounter = null;

        private void Awake() {
            if (!touchCounter) touchCounter = GetComponent<TouchCounterComponent>();
        }

        public void OnCollision(){
            if (isClient) {
                touchCounter.IncreaseCount();
            }
        }


    }
}
