using Clock.Networking;
using Clock.Services;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

namespace Clock
{
    internal class Core : MonoBehaviour
    {


        private static Core instance = null;
        public static Core Instance { get
            {
                if (!instance)
                {
                    if (instance == null)
                    {
                        instance = FindFirstObjectByType<Core>();
                        if (instance == null)
                        {
                            GameObject go = new GameObject("CORE");
                            instance = go.AddComponent<Core>();
                            DontDestroyOnLoad(go);
                        }
                    }
                }
                return instance;
            }
        }

        private ServiceProvider serviceProvider; 
        [HideInInspector] public bool IsLoaded = false;

        private void Awake()
        {
            serviceProvider = new();
            serviceProvider
                .RegisterService(new NetworkingService())
                .RegisterService(new GameModeService());

            IsLoaded = true;
        }

        public T GetService<T>() =>
            serviceProvider.GetService<T>();

        public void GameModeChange()
        {
            GameModeService gameModeService = serviceProvider.GetService<GameModeService>();
            gameModeService.ChangeState(gameModeService.CurrentGameMode == GameMode.Ordinary ? GameMode.Edit : GameMode.Ordinary);
        }

    }
}
