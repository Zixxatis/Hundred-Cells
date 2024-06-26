using System;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class BonusInformer : MonoBehaviour
    {
        [SerializeField] private ColorMimic helperTitleColorMimic;
        [SerializeField] private TextLocalizer helperTitleLTMP;
        [Space]
        [SerializeField] private TextLocalizer helperInfoLTMP;

        private Func<BonusType, BonusInfoSO> getBonusInfo;

        [Inject]
        private void Construct(ResourceSystem resourceSystem)
        {
            this.getBonusInfo = (x) => resourceSystem.BonusesRSS.GetBonusInfo(x);
        }

        private void Start() => this.DeactivateGameObject();

        public void ShowHelperInfo(BonusType bonusType)
        {
            BonusInfoSO bonusInfo = getBonusInfo.Invoke(bonusType);

            if(bonusInfo.ShouldShowBonusHelperInfo == false)
                return;

            this.ActivateGameObject();

            helperTitleColorMimic.ChangeColor(bonusInfo.MatchingCellColor);
            helperTitleLTMP.SetKeyAndUpdate(bonusInfo.TitleLK);

            helperInfoLTMP.SetKeyAndUpdate(bonusInfo.BonusHelperInfoLK);
        }

        public void HideHelperInfo() => this.DeactivateGameObject();
    }
}