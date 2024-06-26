using System;
using Zenject;

namespace CGames
{
    public class PlayerPreferences : ISavable<ConfigData>
    {
        public DraggingDistance DraggingDistance { get; private set; }

        public float CurrentDraggingHeightValue => getDraggingDistanceData.Invoke(DraggingDistance).DistanceInPixels;
        public string CurrentDraggingDistanceLK => getDraggingDistanceData.Invoke(DraggingDistance).DraggingDistanceInfoLK.LocalizationKey;

        private Func<DraggingDistance, DraggingDistanceData> getDraggingDistanceData;

        [Inject]
        private void Construct(ResourceSystem resourceSystem)
        {
            this.getDraggingDistanceData = (x) => resourceSystem.ShapesRSS.GetDraggingDistanceData(x);
        }

        public void ReceiveData(ConfigData data)
        {
            this.DraggingDistance = data.DraggingDistance;
        }

        public void PassData(ConfigData data)
        {
            data.DraggingDistance = this.DraggingDistance;
        }
        
        public void ChangeDraggingDistance(int draggingDistanceIndex) => ChangeDraggingDistance((DraggingDistance)draggingDistanceIndex);

        public void ChangeDraggingDistance(DraggingDistance draggingDistance)
        {
            this.DraggingDistance = draggingDistance;
        }
    }
}