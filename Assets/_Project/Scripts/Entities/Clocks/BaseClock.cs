using Clock.Utilities;
using System;
using UnityEngine;

namespace Clock.Entities.Clocks
{
    internal abstract class BaseClock : MonoBehaviour
    {
        public ObservVariable<DateTime> ChangeTime = new();
        public abstract void SetTime(DateTime dateTime);
        public abstract void Init();
    }
}
