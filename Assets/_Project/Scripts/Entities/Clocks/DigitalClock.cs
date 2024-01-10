using UnityEngine;
using System;
using TMPro;
using System.Collections;

namespace Clock.Entities.Clocks
{
    internal class DigitalClock : BaseClock, IGameModeDependence
    {
        public event Action<DateTime> OnValueChangeEvent;

        [SerializeField] private TMP_InputField hourIF;
        [SerializeField] private TMP_InputField minuteIF;
        [SerializeField] private TMP_InputField secondsIF;

        private Core core { get => Core.Instance; }
        private GameModeService gameModeService;
        private bool isInitialized = false;
        private bool isSelfSetting = false;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => core.IsLoaded);
            gameModeService = core.GetService<GameModeService>();
            gameModeService.Register(this);
            SetReadOnly(gameModeService.CurrentGameMode == GameMode.Ordinary);

            hourIF.onValueChanged.AddListener(OnHourValueChangeHandler);
            minuteIF.onValueChanged.AddListener(OnValueMinuteChangeHandler);
            secondsIF.onValueChanged.AddListener(OnValueSecondChangeHandler);
        }


        public override void Init()
        {
            hourIF.text = "00";
            minuteIF.text = "00";
            secondsIF.text = "00";
            isInitialized = true;
        }


        public override void SetTime(DateTime dateTime)
        {
            isSelfSetting = true;

            int hour = dateTime.Hour;
            if (hour > 12)
                hour = hour - 12;
            
            hourIF.text = hour.ToString("D2");
            minuteIF.text = dateTime.ToString("mm");
            secondsIF.text = dateTime.ToString("ss");

            isSelfSetting = false;
        }

        public void OnGameStateChange(GameMode currentGameMode)
        {
            SetReadOnly(gameModeService.CurrentGameMode == GameMode.Ordinary);
        }

        private void OnValueSecondChangeHandler(string arg0)
        {
            int value = Validate(arg0, 60);
            secondsIF.text = value.ToString("D2");
            InvokeValueChange();
        }

        private void OnValueMinuteChangeHandler(string arg0)
        {
            int value = Validate(arg0, 60);
            minuteIF.text = value.ToString("D2");
            InvokeValueChange();
        }

        private void OnHourValueChangeHandler(string arg0)
        {
            int value = Validate(arg0, 12);
            hourIF.text = value.ToString("D2");
            InvokeValueChange();
        }

        private int Validate(string str, int maxValue)
        {
            int value = int.Parse(str);
            if (value >= maxValue)
            {
                value = maxValue - 1;
            }
            return value;
        }

        private void SetReadOnly(bool value)
        {
            hourIF.readOnly = value;
            minuteIF.readOnly = value;
            secondsIF.readOnly = value;
        }

        private void InvokeValueChange()
        {
            if (!isInitialized || isSelfSetting) return;

            int year = 1;
            int month = 1;
            int day = 1;
            int hour = int.Parse(hourIF.text);
            int minute = int.Parse(minuteIF.text);
            int seconds = int.Parse(secondsIF.text);

            DateTime dateTime = new DateTime(year, month, day, hour, minute, seconds, 1);
            OnValueChangeEvent?.Invoke(dateTime);
        }
    }
}
