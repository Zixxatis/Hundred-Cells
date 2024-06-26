using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace CGames
{
    public class BonusSystem : ISavable<PlayerData>, ISavable<SessionData>
    {
        public event Action OnBonusDataChanged;

        private Action<BonusType> activateBonusAction;
        private Func<BonusType, BonusInfoSO> getBonusInfoByType;
        private Func<CellColor, BonusInfoSO> getBonusInfoByColor;

        private List<BonusData> bonusesDataList;

        public bool CanUseBonusesThisTurn { get; private set; }
        
        [Inject]
        private void Construct(GridModifier gridModifier, ResourceSystem resourceSystem)
        {
            this.activateBonusAction = gridModifier.ApplyBonus;
            this.getBonusInfoByType = resourceSystem.BonusesRSS.GetBonusInfo;
            this.getBonusInfoByColor = resourceSystem.BonusesRSS.GetBonusInfo;
        }

        void ISavable<PlayerData>.ReceiveData(PlayerData data)
        {
            this.bonusesDataList = SavesHelper.GetCorrectListFromSaveFile(GetDefaultBonusesList() , data.BonusesDataList);
        }

        private static List<BonusData> GetDefaultBonusesList()
        {
            List<BonusData> defaultList = new();

            foreach(BonusType bonusType in Enum.GetValues(typeof(BonusType)))
            {
                defaultList.Add(new(bonusType, 0, 0));
            }

            return defaultList;
        }

        void ISavable<PlayerData>.PassData(PlayerData data)
        {
            data.BonusesDataList = this.bonusesDataList;
        }
    
        void ISavable<SessionData>.ReceiveData(SessionData data)
        {
            this.CanUseBonusesThisTurn = data.CanUseBonusesThisTurn;
        }

        void ISavable<SessionData>.PassData(SessionData data)
        {
            data.CanUseBonusesThisTurn = this.CanUseBonusesThisTurn;
        }

        public void ApplyBonus(BonusType bonusType)
        {
            BonusData bonusData = bonusesDataList.First(x => x.BonusType == bonusType);
            bonusData.RemoveBonus();

            activateBonusAction.Invoke(bonusType);
            OnBonusDataChanged?.Invoke();            
        }

        public void IncreaseCollectedCellsAmount(IEnumerable<Cell> cellsInFilledLines)
        {
            Dictionary<CellColor, int> clearedCellsDictionary = cellsInFilledLines.GroupBy(cell => cell.CellColor)
                                                                                  .ToDictionary(group => group.Key, group => group.Count());

            foreach (KeyValuePair<CellColor, int> kvp in clearedCellsDictionary)
            {
                BonusInfoSO bonusInfo = getBonusInfoByColor.Invoke(kvp.Key);
                BonusData bonusData = bonusesDataList.First(x => x.BonusType == bonusInfo.BonusType);

                int collectedCellsAmount = bonusData.ClearedCellsProgress;
                collectedCellsAmount += kvp.Value;

                while(collectedCellsAmount >= bonusInfo.CellsClearedTarget)
                {
                    bonusData.GainBonus(1);
                    collectedCellsAmount -= bonusInfo.CellsClearedTarget;
                }

                bonusData.SetCellsProgressValue(collectedCellsAmount);
            }

            OnBonusDataChanged?.Invoke();
        }

        public void IncreaseBonusesAmount(BonusType bonusType, int amount)
        {
            if(amount <= 0)
                return;

            BonusData bonusData = bonusesDataList.First(x => x.BonusType == bonusType);
            bonusData.GainBonus(amount);

            OnBonusDataChanged?.Invoke();
        }

        public void AllowToUseBonuses() => CanUseBonusesThisTurn = true;
        public void ForbidToUseBonuses() => CanUseBonusesThisTurn = false;

        public BonusData GetBonusData(BonusType bonusType) => bonusesDataList.First(x => x.BonusType == bonusType);
        public BonusInfoSO GetBonusInfo(BonusType bonusType) => getBonusInfoByType.Invoke(bonusType);
    }
}