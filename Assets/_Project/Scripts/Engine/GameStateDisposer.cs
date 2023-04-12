using Mirror;
using System.Collections.Generic;

namespace Engine
{
    internal class GameStateDisposer : NetworkBehaviour
    {
        public GameState CurrentGameState { get; private set; }
        private List<IGameStateDependent> stateDependents = new List<IGameStateDependent>();

        public void Register(IGameStateDependent stateDependent) {
            if (!stateDependents.Contains(stateDependent)) 
                stateDependents.Add(stateDependent);
        }
        public void Unregister(IGameStateDependent stateDependent) {
            if (stateDependents.Contains(stateDependent)) 
                stateDependents.Remove(stateDependent);
        }

        [Command]
        public void ChangeState(GameState gameState) {
            if (!isServer) return;

            CurrentGameState = gameState;
            stateDependents.ForEach(el => {
                el.GameStateChanged(CurrentGameState);
            });
        }
    }
}
