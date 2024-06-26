using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

namespace CGames
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextLocalizer : MonoBehaviour, ILocalizable
    {
        [SerializeField] private LocalizationKeyField localizationKeyField;
        [HideInInspector] public bool ShouldUpdateOnStart = true;
        
        public Graphic Graphic => TextMeshProUGUI;

        private TextMeshProUGUI textMeshProUGUIComponent;
        private TextMeshProUGUI TextMeshProUGUI
        {
            get 
            { 
                if (textMeshProUGUIComponent == null)
                    textMeshProUGUIComponent = GetComponent<TextMeshProUGUI>();

                return textMeshProUGUIComponent; 
            }
            set
            {
                textMeshProUGUIComponent = value;
            }
        }

        private List<Func<string>> valuesList = new();

        [Inject]
        private void Construct(LocalizationSystem localizationSystem)
        {
            localizationSystem.AddToLocalizableList(this);
        }

        private void Start()
        {
            if(ShouldUpdateOnStart)
                UpdateText();
        }

        private void SetKey(string newKeyValue) => localizationKeyField.UpdateKey(newKeyValue);
        private void SetValues(List<Func<string>> valuesList) => this.valuesList = valuesList;

        public void SetKeyAndUpdate(string newKeyValue)
        {
            SetKey(newKeyValue);
            UpdateText();
        }

        public void SetKeyAndUpdate(LocalizationKeyField localizationKeyField)
        {
            SetKey(localizationKeyField.LocalizationKey);
            UpdateText();
        }

        public void SetEverythingAndUpdate(string newKeyValue, List<Func<string>> newValuesList)
        {
            SetKey(newKeyValue);
            SetValues(newValuesList);
            UpdateText();
        }

        public void SetEverythingAndUpdate(LocalizationKeyField localizationKeyField, List<Func<string>> newValuesList)
        {
            SetKey(localizationKeyField.LocalizationKey);
            SetValues(newValuesList);
            UpdateText();
        }

        public void SetEverythingAndUpdate(string newKeyValue, Func<string> newValue)
        {
            SetKey(newKeyValue);
            SetValues(new(){newValue});
            UpdateText();
        }

        public void SetEverythingAndUpdate(LocalizationKeyField localizationKeyField, Func<string> newValue)
        {
            SetKey(localizationKeyField.LocalizationKey);
            SetValues(new(){newValue});
            UpdateText();
        }

        /// <summary> Before setting key and values will check whether they are null or not. </summary>
        /// <remarks> Best for situations where it's unclear if the given values will be null or not. </remarks>
        public void SetEverythingAndUpdate_Safe(string newKeyValue, List<Func<string>> valuesList)
        {
            if(newKeyValue != null)
                SetKey(newKeyValue);
            
            if(valuesList != null && valuesList.Any())
                SetValues(valuesList);

            UpdateText();
        }
        
        /// <summary> Before setting key and values will check whether they are null or not. </summary>
        /// <remarks> Best for situations where it's unclear if the given values will be null or not. </remarks>
        public void SetEverythingAndUpdate_Safe(LocalizationKeyField localizationKeyField, List<Func<string>> valuesList)
        {
            if(localizationKeyField != null)
                SetKey(localizationKeyField.LocalizationKey);
            
            if(valuesList != null && valuesList.Any())
                SetValues(valuesList);
                
            UpdateText();
        }

        /// <summary> Updates text value of the Text Localizer. Gets all values, and changes locale, if Language was changed. </summary>
        public void UpdateText()
        {
            string newText = LocalizationDictionary.GetLocalizedValue(localizationKeyField.LocalizationKey);

            while(newText.Contains('{') && newText.Contains('}'))
                newText = GetStringWithValues(newText);
            
            SetPlainText(newText);
        }

        private string GetStringWithValues(string originalText)
        {
            int startingIndex = originalText.IndexOf('{');
            int endingIndex = originalText.IndexOf('}');

            if (startingIndex < 0 || endingIndex < 0 || endingIndex < startingIndex)
            {
                Debug.LogError($"Couldn't find correct 'start' & 'end' for value field. Object: {name}.");
                return originalText;
            }

            string valueField = originalText.Substring(startingIndex, endingIndex - startingIndex + 1);

            if (int.TryParse(valueField[1..^1], out int valueIndex) == false)
            {
                Debug.LogError($"Given String (\"{originalText}\") could not be parsed. Object: {name}.");
                return originalText;
            }

            if(valuesList.Count - 1 < valueIndex || (valuesList[valueIndex] == null))
            {
                if(Application.isPlaying)
                    Debug.LogWarning($"Couldn't get values for {valueIndex} - Func() is null. Object: {name}.");
                
                return originalText.Replace(valueField, $"[{valueIndex}]");
            }
            else
                return originalText.Replace(valueField, valuesList[valueIndex]());
        }

        /// <summary> Will set the given text to the TMP field. Won't change key. </summary>
        public void SetPlainText(string text) => TextMeshProUGUI.text = text;

        /// <returns> String with the text in TMP field. </returns>
        public string TextFromField => TextMeshProUGUI.text;

        List<LocalizationKeyField> ILocalizable.GetAllLocalizationKeys()
        {
            return new()
            {
                localizationKeyField 
            };
        }
    }
}