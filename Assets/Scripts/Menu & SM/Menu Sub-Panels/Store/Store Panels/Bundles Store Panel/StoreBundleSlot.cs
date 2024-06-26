using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class StoreBundleSlot : MonoBehaviour
    {
        [Header("Visual Elements")]
        [SerializeField] private Image backgroundImage;
  
        [Header("Info Elements")]
        [SerializeField] private TextLocalizer titleLTMP;
        [SerializeField] private TextLocalizer priceLTMP;
        [Space]
        [SerializeField] private TextLocalizer rewardInfoLTMP;
        [SerializeField] private TextLocalizer advertisementInfoLTMP;
        [SerializeField] private TextLocalizer bonusesInfoLTMP;

        [Header("Buttons")]
        [SerializeField] private Button purchaseButton;

        private AdvertisementHandler advertisementHandler;
        private Func<float, string> getPriceString;
        private Action savePlayerData;
        private Action<int> gainCoinsAction;

        private CoinsBundleSO coinsBundle;
        private Action validateValuesInAllSlotsAction;

        [Inject]
        private void Construct(AdvertisementHandler advertisementHandler, LocalizationSystem localizationSystem, SavingSystem savingSystem, Wallet wallet)
        {
            this.advertisementHandler = advertisementHandler;
            this.getPriceString = (x) => localizationSystem.CurrentLanguageInfo.GetPrice(x).ToString();
            this.savePlayerData = savingSystem.SaveData<PlayerData>;
            this.gainCoinsAction = (x) => wallet.GainCoins(x);
        }

        public void Initialize(CoinsBundleSO coinsBundle, Action validateValuesInAllSlotsAction)
        {
            this.coinsBundle = coinsBundle;
            this.validateValuesInAllSlotsAction = validateValuesInAllSlotsAction;

            SetVisuals();
            SetText();
            AddListenersToButton();
            ValidateValues();
        }

        private void SetVisuals()
        {
            backgroundImage.color = coinsBundle.BackgroundColor;
        }

        private void SetText()
        {
            titleLTMP.SetKeyAndUpdate(coinsBundle.TitleLK);
            priceLTMP.SetEverythingAndUpdate("Bundle_Price", () => getPriceString.Invoke(coinsBundle.PriceInUSD));
            
            rewardInfoLTMP.SetEverythingAndUpdate("Bundle_Reward", () => coinsBundle.CoinsAmount.ToString());
            advertisementInfoLTMP.SetKeyAndUpdate("Bundle_Ads");
            bonusesInfoLTMP.SetEverythingAndUpdate("Bundle_Bonus", () => coinsBundle.BonusAmount.ToString());

            if(coinsBundle.IsDisablingAds == false)
                advertisementInfoLTMP.DeactivateGameObject();

            if(coinsBundle.HasBonus == false)
                bonusesInfoLTMP.DeactivateGameObject();
        }

        private void AddListenersToButton()
        {
            purchaseButton.onClick.AddListener(PurchaseBundle);
        }

        private void PurchaseBundle()
        {
            gainCoinsAction.Invoke(GetFullCoinsAmount());
            
            if(coinsBundle.IsDisablingAds)
                advertisementHandler.DisableAds();

            savePlayerData.Invoke();
            validateValuesInAllSlotsAction.Invoke();
        }

        public void ValidateValues()
        {
            priceLTMP.UpdateText();

            if(advertisementHandler.IsAdvertisementsEnabled == false)
            {
                advertisementInfoLTMP.DeactivateGameObject();

                if(bonusesInfoLTMP.gameObject.activeSelf)
                    bonusesInfoLTMP.SetEverythingAndUpdate("Bundle_Bonus", () => coinsBundle.IncreasedBonusValue.ToString());
            }
        }

        private int GetFullCoinsAmount()
        {
            if(advertisementHandler.IsAdvertisementsEnabled)
                return coinsBundle.CoinsAmount + coinsBundle.BonusAmount;
            else
                return coinsBundle.CoinsAmount + coinsBundle.IncreasedBonusValue;
        }

        private void OnDestroy()
        {
            purchaseButton.onClick.RemoveListener(PurchaseBundle);
        }
    }
}