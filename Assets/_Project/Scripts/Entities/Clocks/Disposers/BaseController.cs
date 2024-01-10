using System.Collections.Generic;
using UnityEngine;
using System;

namespace Clock.Entities.Clocks
{
    internal abstract class BaseController : MonoBehaviour
    {
        public event Action<DateTime> OnTimeChangeEvent;

        protected TimeHolder timeHolder;
        protected bool IsInitialized = false;

        public virtual void Initialize(params System.Object[] param)
        {
            if (IsInitialized) return;
            this.timeHolder = param[0] as TimeHolder;
        }

        internal virtual void OnUpdate() { }
        internal virtual void OnChangeController() { }
        protected virtual void InvokeTimeEvent(DateTime dateTime) =>
            OnTimeChangeEvent?.Invoke(dateTime);
    }
}
