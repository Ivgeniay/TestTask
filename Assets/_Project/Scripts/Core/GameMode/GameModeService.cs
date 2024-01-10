using System.Collections.Generic;
using Clock.Services;

namespace Clock
{
    internal class GameModeService : IService
    {
        public GameMode CurrentGameMode;

        private List<IGameModeDependence> dependences = new();
        public void Register(IGameModeDependence dependence)
        {
            if (!dependences.Contains(dependence))
                dependences.Add(dependence);
        }

        public void Unregister(IGameModeDependence dependence)
        {
            if (dependences.Contains(dependence))
                dependences.Remove(dependence);
        }

        public void ChangeState(GameMode gameMode)
        {
            CurrentGameMode = gameMode;
            foreach (var dependence in dependences)
            {
                dependence.OnGameStateChange(gameMode);
            }
        }
    }
}
