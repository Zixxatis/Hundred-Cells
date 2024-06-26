using System;

namespace CGames
{
    public interface IShapePlacedNotifier
    {
        public event Action OnShapePlaced;
    }
}