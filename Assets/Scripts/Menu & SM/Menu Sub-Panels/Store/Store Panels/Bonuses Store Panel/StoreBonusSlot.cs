using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

namespace CGames
{
    public class StoreBonusSlot : MonoBehaviour
    {   
        [Header("Visual Elements")]
        [SerializeField] private Image iconImage;
        [SerializeField] private ColorMimic iconColorMimic;
        [Space]
        [SerializeField] private ColorMimic amountColorMimic;

        [Header("Info Elements")]
        [SerializeField] private TextLocalizer titleLTMP;
        [SerializeField] private TextMeshProUGUI priceTMP;
        [Space]
        [SerializeField] private TextMeshProUGUI currentAmountTMP;

        [Header("Trade Elements")]
        [SerializeField] private TMP_InputField amountInputField;
        [SerializeField, Min(0)] private int minimumBonusesAmount = 1;
        [Space]
        [SerializeField] private Image subIconImage;
        [SerializeField] private ColorMimic subIconColorMimic;
        [Space]
        [SerializeField] private Button purchaseButton;

        private BonusSystem bonusSystem;
        private Wallet wallet;
        private Action validateValuesInAllSlotsAction;

        private Func<byte> getAvailableAmount;
        private BonusInfoSO bonusInfoSO;

        private byte selectedAmount;

        [Inject]
        private void Construct(BonusSystem bonusSystem, Wallet wallet)
        {
            this.bonusSystem = bonusSystem;
            this.wallet = wallet;
        }

        public void Initialize(BonusType bonusType, Action validateValuesInAllSlotsAction)
        {
            this.bonusInfoSO = bonusSystem.GetBonusInfo(bonusType);
            this.validateValuesInAllSlotsAction = validateValuesInAllSlotsAction;
            this.getAvailableAmount = () => bonusSystem.GetBonusData(bonusInfoSO.BonusType).AvailableAmount;

            SetVisuals();
            SetText();
            AssignListeners();
            ResetValues();
        }

        private void SetVisuals()
        {
            iconImage.sprite = bonusInfoSO.Icon;
            subIconImage.sprite = bonusInfoSO.Icon;
            
            iconColorMimic.ChangeColor(bonusInfoSO.MatchingCellColor);
            amountColorMimic.ChangeColor(bonusInfoSO.MatchingCellColor);
            subIconColorMimic.ChangeColor(bonusInfoSO.MatchingCellColor);
        }

        private void SetText()
        {
            titleLTMP.SetKeyAndUpdate(bonusInfoSO.TitleLK);
        }

        private void AssignListeners()
        {
            amountInputField.onSelect.AddListener((x) => ResetValues());
            amountInputField.onDeselect.AddListener(ChangeAmount);
            amountInputField.onSubmit.AddListener(ChangeAmount);
        
            purchaseButton.onClick.AddListener(() => Purchase(selectedAmount));
        }

        private void ChangeAmount(string input)
        {
            if(int.TryParse(input, out int amount))
            {
                if(amount >= minimumBonusesAmount && amount <= byte.MaxValue)
                {
                    selectedAmount = (byte)amount;
                    UpdateValues();

                    return;
                }
            }

            ResetValues();
        }
        
        private void UpdateValues()
        {
            amountInputField.text = selectedAmount.ToString();
            priceTMP.text = $"x{bonusInfoSO.CoinsPrice * selectedAmount}";

            currentAmountTMP.text = $"x{getAvailableAmount()}";

            ValidateValues();
        }

        public void ValidateValues()
        {
            purchaseButton.interactable = wallet.IsEnoughCoinsFor(bonusInfoSO.CoinsPrice * selectedAmount);
        }

        public void ResetValues()
        {
            selectedAmount = 1;
            UpdateValues();
        }

        private void Purchase(int amount)
        {
            wallet.SpendCoins(bonusInfoSO.CoinsPrice * amount);
            bonusSystem.IncreaseBonusesAmount(bonusInfoSO.BonusType, amount);

            UpdateValues();
            validateValuesInAllSlotsAction.Invoke();
        }

        private void OnDestroy()
        {
            amountInputField.onSelect.RemoveAllListeners();
            amountInputField.onDeselect.RemoveListener(ChangeAmount);
            amountInputField.onSubmit.RemoveListener(ChangeAmount);

            purchaseButton.onClick.RemoveAllListeners();
        }
    }
}