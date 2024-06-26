using UnityEngine;

namespace CGames
{
    public interface IDimensionsNotifier
    {
        public Vector2 CellDimensions { get; }
        public Vector2 GridSpacing { get; }
    }
}