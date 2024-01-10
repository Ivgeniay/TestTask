using System.Collections.Generic;
using System.Collections;
using Clock.Networking;
using System.Linq;
using UnityEngine;
using System; 

namespace Clock.Entities.Clocks
{
    internal class ClockDisposer : MonoBehaviour, IGameModeDependence
    {
        [SerializeField] private List<BaseClock> clocks;
        private Core core { get => Core.Instance; }
        private BaseController controller;
        [SerializeField] private AnalogueController analogueController;
        [SerializeField] private TimerController timerController;

        private TimeHolder timeHolder;

        private NetworkingService networkingService;
        private GameModeService gameModeService;

        private void Awake()
        {
            if (clocks == null || clocks.Count == 0) clocks = GetComponentsInChildren<BaseClock>().ToList();
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => Core.Instance.IsLoaded);

            gameModeService = core.GetService<GameModeService>();
            gameModeService.Register(this);
            networkingService = core.GetService<NetworkingService>();

            clocks.ForEach(clock => { clock.Init(); });
            yield return networkingService.SendRequestTimeData(GetNetworkingTimeData());
        }  

        private void Update() =>
            controller?.OnUpdate();
        

        private Action<DateTime> GetNetworkingTimeData()
        {
            return (e) =>
            {
                Debug.Log(e);
                clocks.ForEach(clock =>
                {
                    clock.SetTime(e);
                });

                if (timeHolder == null)
                {
                    timeHolder = new()
                    {
                        CurrentTime = e,
                        StartTime = e,
                    };
                }

                analogueController.Initialize(timeHolder, clocks);
                timerController.Initialize(timeHolder, clocks);

                if (controller != null) controller.OnTimeChangeEvent -= OnTimeChangeHandlet;
                controller = timerController;
                controller.OnTimeChangeEvent += OnTimeChangeHandlet;
            };
        } 

        public void OnGameStateChange(GameMode currentGameMode)
        {
            switch (currentGameMode)
            {
                case GameMode.Ordinary:
                    controller.OnTimeChangeEvent -= OnTimeChangeHandlet;
                    controller = timerController;
                    controller.OnChangeController();
                    controller.OnTimeChangeEvent += OnTimeChangeHandlet;
                    break;
                case GameMode.Edit:

                    timeHolder.StartTime = timeHolder.CurrentTime;

                    controller.OnTimeChangeEvent -= OnTimeChangeHandlet;
                    controller = analogueController;
                    controller.OnChangeController();
                    controller.OnTimeChangeEvent += OnTimeChangeHandlet;
                    break;
            }
        }

        private void OnTimeChangeHandlet(DateTime time)
        {
            clocks.ForEach(e =>
            {
                e.SetTime(time);
            });
        }
    }


    public class TimeHolder
    {
        public DateTime StartTime;
        public DateTime CurrentTime;
    }
}
