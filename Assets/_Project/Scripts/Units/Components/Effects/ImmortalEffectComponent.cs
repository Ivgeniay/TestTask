using System.Collections;
using Units.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Units.Components.Effects
{
    [RequireComponent(typeof(TakeDamageComponent))]
    internal class ImmortalEffectComponent : BaseEffectComponent
    {
        public bool IsImmortal { get; private set; }

        [SerializeField] private float immortalDuration;

        private TakeDamageComponent healthComponent;

        private void Awake() {
            healthComponent = GetComponent<TakeDamageComponent>();
        }

        public override void StartEffect() {
            IsImmortal = true;
            OnEffectStarted?.Invoke();
            healthComponent.SetImmortal(IsImmortal);
            StartCoroutine(MakeImmortalRoutine(immortalDuration));
        }

        private IEnumerator MakeImmortalRoutine(float seconds) {
            yield return new WaitForSeconds(seconds);
            IsImmortal = false;
            healthComponent.SetImmortal(IsImmortal);
            OnEffectEnded?.Invoke();
        }
    }
}
