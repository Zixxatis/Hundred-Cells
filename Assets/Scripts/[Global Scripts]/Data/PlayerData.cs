using System;
using System.Collections.Generic;

namespace CGames
{
    [Serializable]
    public class PlayerData : Data
    {
        public override byte Version => 1;
        public override string LogPrefix => "PLAYER";
        protected override string FileName => "player.data";

        // Global
        public byte SaveVersion { get; set; }

        // Currency
        public ushort Coins { get; set; }

        // Bonuses
        public List<BonusData> BonusesDataList { get; set; }

        // Color Collection
        public Dictionary<ColorsCollectionType, bool> CollectionsUnlockStatusDictionary { get; set; }
        public ColorsCollectionType SelectedCollection { get; set; }

        // Advertisements
        public bool IsAdvertisementsEnabled { get; set; }

        public PlayerData()
        {        
            SaveVersion = Version;

            Coins = 0;

            BonusesDataList = new();

            CollectionsUnlockStatusDictionary = new();
            SelectedCollection = ColorsCollectionType.Classic;

            IsAdvertisementsEnabled = true;
        }
    }
}