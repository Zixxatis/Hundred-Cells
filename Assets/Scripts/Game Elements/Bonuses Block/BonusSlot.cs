using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using System;

namespace CGames
{
    public class BonusSlot : MonoBehaviour
    {
        [SerializeField] private Button bonusButton;
        [Space]
        [SerializeField] private Image sliderImage;
        [SerializeField] private ColorMimic sliderColorMimic;
        [Space]
        [SerializeField] private Image iconImage;
        [SerializeField] private ColorMimic iconColorMimic;
        [Space]
        [SerializeField] private TextMeshProUGUI bonusAmountTMP;

        public BonusType BonusType { get; private set; }

        private BonusSystem bonusSystem;
        private BonusInfoSO bonusInfo;

        [Inject]
        private void Construct(BonusSystem bonusSystem)
        {
            this.bonusSystem = bonusSystem;
        }

        public void Initialize(BonusType bonusType, Action disableAllBonusButtons)
        {
            bonusSystem.OnBonusDataChanged += UpdateBonusDataOutput;

            bonusButton.onClick.AddListener(ApplyBonus);
            bonusButton.onClick.AddListener(disableAllBonusButtons.Invoke);

            BonusType = bonusType;
            bonusInfo = bonusSystem.GetBonusInfo(bonusType);

            SetDefaultVisuals();
            UpdateBonusDataOutput();
        }

        private void SetDefaultVisuals()
        {
            iconImage.sprite = bonusInfo.Icon;

            iconColorMimic.ChangeColor(bonusInfo.MatchingCellColor);
            sliderColorMimic.ChangeColor(bonusInfo.MatchingCellColor);
        }

        private void UpdateBonusDataOutput()
        {
            BonusData bonusData = bonusSystem.GetBonusData(BonusType);

            bonusAmountTMP.text = bonusData.AvailableAmount.ToString();
            sliderImage.fillAmount = bonusInfo.GetProgressFill(bonusData.ClearedCellsProgress);

            TryToEnableInteractivity();
        }

        public void TryToEnableInteractivity()
        {
            bonusButton.interactable = bonusSystem.GetBonusData(BonusType).AvailableAmount > 0;
            ChangeTextVisibility();
        }

        public void DisableInteractivity()
        {
            bonusButton.interactable = false;
            ChangeTextVisibility();
        }

        private void ChangeTextVisibility() => bonusAmountTMP.ChangeGameObjectActivation(bonusButton.interactable);

        private void ApplyBonus() => bonusSystem.ApplyBonus(BonusType);

        private void OnDestroy()
        {
            bonusSystem.OnBonusDataChanged -= UpdateBonusDataOutput;            
            bonusButton.onClick.RemoveAllListeners();
        }
    }
}