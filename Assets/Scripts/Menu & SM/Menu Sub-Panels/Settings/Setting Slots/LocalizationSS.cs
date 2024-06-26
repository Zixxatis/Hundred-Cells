using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class LocalizationSS : SettingSlot
    {
        [Header("Settings - Locale")]
        [SerializeField] private Button leftLocaleButton;
        [SerializeField] private Image flagImage;
        [SerializeField] private Button rightLocaleButton;
        
        private LocalizationSystem localizationSystem;
        private Func<int, LanguageInfoSO> getLanguageInfo;

        private int currentLanguageIndex;

        [Inject]
        private void Construct(LocalizationSystem localizationSystem, ResourceSystem resourceSystem)
        {
            this.localizationSystem = localizationSystem;
            this.getLanguageInfo = resourceSystem.LocalizationRSS.GetLanguageInfoSO;
        }

        public override void PrepareSettingSlot()
        {
            leftLocaleButton.onClick.AddListener(DecreaseLocalizationIndex);
            rightLocaleButton.onClick.AddListener(IncreaseLocalizationIndex);
        }

        public override void MatchValuesToCurrent()
        {
            currentLanguageIndex = (int)localizationSystem.Language;
            flagImage.sprite = getLanguageInfo(currentLanguageIndex).FlagSprite;
        }

        private void DecreaseLocalizationIndex()
        {
            currentLanguageIndex--;

            if(currentLanguageIndex == -1)
                currentLanguageIndex = Enum.GetValues(typeof(Language)).Length - 1;

            ChangeLocale();
        }

        private void IncreaseLocalizationIndex()
        {
            currentLanguageIndex++;
                
            if(currentLanguageIndex == Enum.GetValues(typeof(Language)).Length)
                currentLanguageIndex = 0;

            ChangeLocale();
        }

        private void ChangeLocale()
        {
            flagImage.sprite = getLanguageInfo(currentLanguageIndex).FlagSprite;
            localizationSystem.ChangeLanguage(currentLanguageIndex);
        }

        private void OnDestroy()
        {
            leftLocaleButton.onClick.RemoveListener(DecreaseLocalizationIndex);
            rightLocaleButton.onClick.RemoveListener(IncreaseLocalizationIndex);
        }
    }
}