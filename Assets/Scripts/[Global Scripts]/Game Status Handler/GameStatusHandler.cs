using System;

namespace CGames
{
    public class GameStatusHandler : ISavable<SessionData>, IGameOverNotifier
    {
        public event Action OnGameOver;

        public GameMode GameMode { get; private set; }

        public void ReceiveData(SessionData data)
        {
            GameMode = data.GameMode;
        }

        public void PassData(SessionData data)
        {
            data.GameMode = GameMode;
        }
        
        public void FinishSession()
        {
            OnGameOver?.Invoke();
        }

        public void ChangeGameMode(GameMode gameMode) => this.GameMode = gameMode;
    }
}
