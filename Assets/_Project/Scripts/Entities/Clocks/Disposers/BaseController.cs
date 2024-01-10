using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clock.Entities.Clocks
{
    internal abstract class BaseController : MonoBehaviour
    {
        public event Action<DateTime> OnTimeChangeEvent;

        protected TimeHolder timeHolder;
        protected List<BaseClock> clocks;
        protected bool IsInitialized = false;

        public virtual void Initialize(TimeHolder timeHolder, List<BaseClock> clocks)
        {
            if (IsInitialized) return;

            this.timeHolder = timeHolder;
            this.clocks = clocks;
        }

        internal virtual void OnUpdate() { }
        internal virtual void OnChangeController() { }
        protected virtual void InvokeTimeEvent(DateTime dateTime) =>
            OnTimeChangeEvent?.Invoke(dateTime);
    }
}
