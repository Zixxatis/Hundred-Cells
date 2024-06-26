using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CGames
{
    [CreateAssetMenu(fileName = "Dragging Distance Config", menuName = "Configs/Dragging Distance Config", order = 0)]
    public class DraggingDistanceConfig : ScriptableObject, ILocalizable
    {
        [SerializeField] private List<DraggingDistanceData> draggingDistancesDataList;

        public DraggingDistanceData GetDraggingDistanceData(DraggingDistance draggingDistance)
        {
            return draggingDistancesDataList.First(x => x.DraggingDistance == draggingDistance);
        }

        List<LocalizationKeyField> ILocalizable.GetAllLocalizationKeys()
        {
            return draggingDistancesDataList.Select(x => x.DraggingDistanceInfoLK).ToList();
        }
    }
}