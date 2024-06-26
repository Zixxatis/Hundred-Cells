using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace CGames
{
    public class CollectionsInventory : ISavable<PlayerData>
    {
        public event Action OnCollectionChanged;
        public CellColorsCollection SelectedCollection => getColorCollection.Invoke(selectedCollectionType);
        
        private Func<ColorsCollectionType, CellColorsCollection> getColorCollection;
        private Dictionary<ColorsCollectionType, bool> collectionsUnlockStatusDictionary;
        private ColorsCollectionType selectedCollectionType;

        [Inject]
        private void Construct(ResourceSystem resourceSystem)
        {
            this.getColorCollection = (x) => resourceSystem.ColorsRSS.GetCellColorsCollection(x);
        }

        public void ReceiveData(PlayerData data)
        {
            collectionsUnlockStatusDictionary = SavesHelper.GetCorrectDictionaryFromSaveFile(GetDefaultCollectionsDictionary() , data.CollectionsUnlockStatusDictionary);
            selectedCollectionType = data.SelectedCollection;
        }

        private static Dictionary<ColorsCollectionType, bool> GetDefaultCollectionsDictionary()
        {
            Dictionary<ColorsCollectionType, bool> defaultDictionary = Enum.GetValues(typeof(ColorsCollectionType)).Cast<ColorsCollectionType>().ToDictionary(x => x, x => false);
            defaultDictionary[ColorsCollectionType.Classic] = true;

            return defaultDictionary;
        }

        public void PassData(PlayerData data)
        {
            data.CollectionsUnlockStatusDictionary = collectionsUnlockStatusDictionary;
            data.SelectedCollection = selectedCollectionType;
        }

        public void ChangeCollection(ColorsCollectionType newCollectionType)
        {
            if(selectedCollectionType == newCollectionType)
                return;

            selectedCollectionType = newCollectionType;
            OnCollectionChanged?.Invoke();
        }

        public void UnlockCollection(ColorsCollectionType unlockedCollectionType)
        {
            if(collectionsUnlockStatusDictionary[unlockedCollectionType] == false)
                collectionsUnlockStatusDictionary[unlockedCollectionType] = true;
        }

        public bool IsCollectionUnlocked(ColorsCollectionType colorsCollectionType) => collectionsUnlockStatusDictionary[colorsCollectionType];
        public bool IsCollectionSelected(ColorsCollectionType colorsCollectionType) => selectedCollectionType == colorsCollectionType;
    }
}