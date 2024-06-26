using System;

namespace CGames
{
    public interface IGameModeEnteredNotifier
    {
        public event Action OnGameModeEntered;
    }
}