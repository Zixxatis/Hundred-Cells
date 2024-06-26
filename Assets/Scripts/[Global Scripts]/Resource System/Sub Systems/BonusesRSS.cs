using System.Collections.Generic;
using System.Linq;

namespace CGames
{
    public class BonusesRSS
    {
        private readonly List<BonusInfoSO> bonusesInfoList;

        public BonusesRSS(List<BonusInfoSO> bonusesInfoList)
        {
            this.bonusesInfoList = bonusesInfoList;
            bonusesInfoList.Sort((x, y) => x.BonusType.CompareTo(y.BonusType));
        }

        public BonusInfoSO GetBonusInfo(BonusType bonusType) => bonusesInfoList.First(x => x.BonusType == bonusType);
        public BonusInfoSO GetBonusInfo(CellColor cellColor) => bonusesInfoList.First(x => x.MatchingCellColor == cellColor);
    }
}