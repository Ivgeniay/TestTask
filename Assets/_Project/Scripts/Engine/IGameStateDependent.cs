namespace Engine
{
    internal interface IGameStateDependent
    {
        public void GameStateChanged(GameState currentGameState);
    }
}
