using Clock.Entities.Arrows;
using Clock.Utilities;
using System;
using UnityEngine;

namespace Clock.Entities.Clocks
{
    internal class AnalogueClock : BaseClock
    {
        [SerializeField] private ArrowView hourArrow;
        [SerializeField] private ArrowView minuteArrow;
        [SerializeField] private ArrowView secondArrow;

        public ArrowController HourController { get; private set; }
        public ArrowController MinuteController { get; private set; }
        public ArrowController SecondsController { get; private set; }

        public override void SetTime(DateTime dateTime)
        {
            HourController.SetPosition(dateTime.Hour, dateTime.Minute);
            MinuteController.SetPosition(dateTime.Minute, dateTime.Second);
            SecondsController.SetPosition(dateTime.Second, 0);
        }

        public override void Init()
        {
            HourController = new ArrowController(hourArrow, 12);
            MinuteController = new ArrowController(minuteArrow, 60);
            SecondsController = new ArrowController(secondArrow, 60);
        }
    }
}
