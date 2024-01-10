using Clock.Utilities;
using UnityEngine;

namespace Clock.Entities.Timers
{
    internal class Timer
    {
        public ObservVariable<float> SecondsSinceStart { get; private set; } = new();
        private float secondsOffset { get; set; }

        public void Update()
        {
            secondsOffset += Time.unscaledDeltaTime;
            if (secondsOffset > 1)
            {
                SecondsSinceStart.Value += secondsOffset;
                secondsOffset = 0;
            }
        }
        
        public void Reset()
        {
            SecondsSinceStart.Value = 0;
            secondsOffset = 0;
        }
    }
}
