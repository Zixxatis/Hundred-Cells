using System;

namespace CGames
{
    public interface IGameOverNotifier
    {
        public event Action OnGameOver;
    }
}