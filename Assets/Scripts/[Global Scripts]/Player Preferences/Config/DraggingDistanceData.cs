using System;
using UnityEngine;

namespace CGames
{
    [Serializable]
    public class DraggingDistanceData
    {
        [field: SerializeField] public DraggingDistance DraggingDistance { get; private set; }
        [field: Space]
        [field: SerializeField] public float DistanceInPixels { get; private set; }
        [field: SerializeField] public LocalizationKeyField DraggingDistanceInfoLK { get; private set; }
    }
}