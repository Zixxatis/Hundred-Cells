using System;

namespace CGames
{   
    [Serializable]
    public class BonusData
    {
        public BonusType BonusType { get; private set; }
        public byte AvailableAmount { get; private set; }
        public int ClearedCellsProgress { get; private set; }
        
        public BonusData(BonusType bonusType, byte availableAmount, int clearedCellsProgress)
        {
            BonusType = bonusType;
            AvailableAmount = availableAmount;
            ClearedCellsProgress = clearedCellsProgress;
        }

        public void GainBonus(int gainAmount)
        {
            if(AvailableAmount + gainAmount > byte.MaxValue)
                AvailableAmount = byte.MaxValue;
            else
                AvailableAmount += (byte)gainAmount;
        }

        public void RemoveBonus()
        {
            if(AvailableAmount > 0)
                AvailableAmount--;
        }

        public void SetCellsProgressValue(int newProgressValue) => ClearedCellsProgress = newProgressValue;

        
    }
}