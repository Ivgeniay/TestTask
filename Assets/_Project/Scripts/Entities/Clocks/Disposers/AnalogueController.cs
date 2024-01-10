using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clock.Entities.Clocks
{
    internal class AnalogueController : BaseController
    {
        [SerializeField] private ArrowView hourArrowView;
        [SerializeField] private ArrowView minuteArrowView;
        [SerializeField] private ArrowView secondArrowView;

        public override void Initialize(TimeHolder timeHolder, List<BaseClock> clocks)
        {
            base.Initialize(timeHolder, clocks);
            if (IsInitialized) return;

            clocks.ForEach(e =>
            {
                if (e is AnalogueClock a)
                {
                    a.HourController.OnPointerMovedEvent += OnAnalogueValueChangeHandler;
                    a.MinuteController.OnPointerMovedEvent += OnAnalogueValueChangeHandler;
                    a.SecondsController.OnPointerMovedEvent += OnAnalogueValueChangeHandler;
                }
                else if (e is DigitalClock d)
                {
                    d.OnValueChangeEvent += OnDigitalValueChangeHandler;
                }
            });

            IsInitialized = true;
        }

        private void OnDigitalValueChangeHandler(DateTime obj)
        {
            int year = timeHolder.CurrentTime.Year;
            int month = timeHolder.CurrentTime.Month;
            int day = timeHolder.CurrentTime.Day;
            int hour = obj.Hour;
            int minute = obj.Minute;
            int seconds = obj.Second;

            timeHolder.CurrentTime = new DateTime(year, month, day, hour, minute, seconds);
            timeHolder.StartTime = new DateTime(year, month, day, hour, minute, seconds);

            clocks.ForEach(e => { e.SetTime(timeHolder.CurrentTime); });
        }

        private void OnAnalogueValueChangeHandler(object sender, int circlePart, float value)
        {
            switch (sender)
            {
                case ArrowView arrow:
                    try
                    {
                        var res = GetPart(value, circlePart);

                        int year = timeHolder.CurrentTime.Year;
                        int month = timeHolder.CurrentTime.Month;
                        int day = timeHolder.CurrentTime.Day;
                        int hour = timeHolder.CurrentTime.Hour;
                        int minute = timeHolder.CurrentTime.Minute;
                        int seconds = timeHolder.CurrentTime.Second;

                        if (arrow == hourArrowView)
                        {
                            hour = res.Real;
                            minute = GetIntLepr(res.Fraction, 0, 59);
                            seconds = GetIntLepr((res.Fraction * 100) % 1, 0, 59);
                        }
                        else if (arrow == minuteArrowView)
                        {
                            minute = res.Real;
                            seconds = GetIntLepr(res.Fraction, 0, 59);
                        }
                        else if (arrow == secondArrowView)
                            seconds = res.Real;
                    
                        Debug.Log($"Sender: {sender}, CirclePart: {circlePart} Value: {value}");

                        timeHolder.CurrentTime = new DateTime(year, month, day, hour, minute, seconds);
                        timeHolder.StartTime = new DateTime(year, month, day, hour, minute, seconds);

                        clocks.ForEach(e => { e.SetTime(timeHolder.CurrentTime); });

                    }
                    catch (Exception ex) 
                    {
                        Debug.LogException(ex);
                    } 
                    break;

            }
        }

        private (int Real, float Fraction) GetPart(float angle, int circlePart)
        {
            angle = MathF.Abs(angle - 360);
            float t = angle / 360f;
            float r = t * circlePart;
            float fraction = r % 1;
            int real = Mathf.RoundToInt(r - fraction);
            return (real, fraction);
        }

        private int GetIntLepr(float fraction, int min, int max) =>
            Mathf.RoundToInt(Mathf.Lerp(min, max, fraction));
    }
}
