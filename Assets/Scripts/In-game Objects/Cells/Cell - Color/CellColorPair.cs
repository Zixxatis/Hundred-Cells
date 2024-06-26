using System;
using UnityEngine;

namespace CGames
{
    [Serializable]
    public class CellColorPair
    {
        [field: SerializeField] public CellColor CellColor { get; private set; }
        [field: SerializeField] public Color32 Color32 { get; private set; }

        public CellColorPair(CellColor cellColor, Color32 color32)
        {
            CellColor = cellColor;
            Color32 = color32;
        }
    }
}
