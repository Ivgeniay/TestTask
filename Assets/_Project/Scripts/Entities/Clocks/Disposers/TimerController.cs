using System.Collections.Generic; 
using Clock.Entities.Timers;

namespace Clock.Entities.Clocks
{
    internal class TimerController : BaseController
    {
        private Timer timer;
        public override void Initialize(TimeHolder timeHolder, List<BaseClock> clocks)
        {
            base.Initialize(timeHolder, clocks);
            if (IsInitialized) return;

            timer = new Timer();
            timer.SecondsSinceStart.OnChangeEvent += TimerOnChangeHandler;

            IsInitialized = true;
        }

        internal override void OnChangeController()
        {
            timer.Reset();
        }

        private void TimerOnChangeHandler(float timeOffset)
        {
            timeHolder.CurrentTime = timeHolder.StartTime.AddSeconds(timeOffset);
            InvokeTimeEvent(timeHolder.CurrentTime);
        }

        internal override void OnUpdate()
        {
            timer?.Update();
        }
    }
}
