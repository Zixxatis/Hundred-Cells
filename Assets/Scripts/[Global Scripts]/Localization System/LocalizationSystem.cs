using System;
using System.Collections.Generic;
using Zenject;

namespace CGames
{
    public class LocalizationSystem : ISavable<ConfigData>, IInitializable
    {
        public Language Language { get; private set; }
        public LanguageInfoSO CurrentLanguageInfo => getLanguageInfoSO(Language);

        private readonly List<TextLocalizer> savingSystemObjects = new();
        private readonly Func<Language, LanguageInfoSO> getLanguageInfoSO;
        
        public LocalizationSystem(ResourceSystem resourceSystem)
        {
            this.getLanguageInfoSO = (x) => resourceSystem.LocalizationRSS.GetLanguageInfoSO(x);

            LocalizationDictionary.Initialize(() => Language);
        }

        public void ReceiveData(ConfigData data)
        {
            Language = data.Language;
        }

        public void PassData(ConfigData data)
        {
            data.Language = Language;
        }

        public void Initialize()
        {
            UpdateAllLocalizationFields();
        }

        private void UpdateAllLocalizationFields()
        {
            foreach (TextLocalizer script in savingSystemObjects)
            {
                script.UpdateText();
            }
        }

        public void AddToLocalizableList(TextLocalizer textLocalizer)
        {
            savingSystemObjects.Add(textLocalizer);
        }
        
        public void ChangeLanguage(Language newLanguage)
        {
            if(Language == newLanguage)
                return;

            Language = newLanguage;
            UpdateAllLocalizationFields();
        }

        public void ChangeLanguage(int languageIndex) => ChangeLanguage((Language)languageIndex);
    }
}