using System;

namespace CGames
{
    public class AdvertisementHandler : ISavable<PlayerData>
    {
        public event Action OnAdvertisementsRemoved;
        
        public bool IsAdvertisementsEnabled { get; private set; }

        public void ReceiveData(PlayerData data)
        {
            IsAdvertisementsEnabled = data.IsAdvertisementsEnabled;
        }

        public void PassData(PlayerData data)
        {
            data.IsAdvertisementsEnabled = IsAdvertisementsEnabled;
        }

        public void DisableAds()
        {
            if(IsAdvertisementsEnabled == false)
                return;

            IsAdvertisementsEnabled = false;
            OnAdvertisementsRemoved?.Invoke();
        }
    }
}