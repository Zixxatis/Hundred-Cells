using System;

namespace CGames
{
    public interface INewGameStartedNotifier
    {
        public event Action OnNewGameStarted;
    }
}