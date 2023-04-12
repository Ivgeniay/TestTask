using UnityEngine;
using UnityEngine.Events;

namespace Units.Components.Effects

{
    internal abstract class BaseEffectComponent : MonoBehaviour
    {
        public UnityEvent OnEffectStarted;
        public UnityEvent OnEffectEnded;
        public abstract void StartEffect();
    }
}
