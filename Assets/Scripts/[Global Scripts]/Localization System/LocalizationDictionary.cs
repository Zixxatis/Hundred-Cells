using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CGames
{
    public static class LocalizationDictionary
    {
        private static readonly string separator = $"\",\"";
        private static readonly Dictionary<Language, Dictionary<string, string>> mainDictionary = new();
        private static TextAsset dictionaryFile;

        private static DateTime? lastLoadTime = null;
        private static DateTime DictionaryLastWriteTime => File.GetLastWriteTime(Path.Combine(Application.dataPath, "Resources/") + "Localization.csv");
        private static Func<Language> getSelectedLanguage;

        public static void Initialize(Func<Language> getSelectedLanguage)
        {
            LocalizationDictionary.getSelectedLanguage = getSelectedLanguage;

            FillDictionary();
        }

        private static void FillDictionary()
        {
            dictionaryFile = Resources.Load<TextAsset>("Localization");
            lastLoadTime = DateTime.Now;

            UpdateDictionary();
        }

        public static void UpdateDictionary()
        {
            string[] dictionaryFileRows = dictionaryFile.ToString().Split('\n');

            if(mainDictionary.Any())
                mainDictionary.Clear();
            
            string[] headers = dictionaryFileRows[0].Split(separator, StringSplitOptions.None);

            for (int i = 1; i < headers.Length; i++)
            {
                Language language = (Language)(i - 1);

                if(headers[i].Contains(language.ToString()))
                    mainDictionary.Add(language, new());
            }

            for (int i = 1; i < dictionaryFileRows.Length; i++)
            {
                string line = dictionaryFileRows[i];

                if(string.IsNullOrEmpty(line))
                    continue;
                else
                {
                    line = line.TrimStart('"');
                    line = line.TrimEnd('\r');
                    line = line.TrimEnd('"');
                }

                string[] keys = line.Split(separator);

                for (int j = 1; j < keys.Length; j++)
                {
                    string key = keys[0];
                    
                    if(mainDictionary[(Language)j - 1].ContainsKey(key) == false)
                    {
                        string value = keys[j];
                        value = value.Replace("\\n", "\n");

                        mainDictionary[(Language)j - 1].Add(key, value);
                    }
                    else
                        Debug.LogWarning($"Got a duplicate key: {key}. Skipped it.");
                }                
            }
        }

        public static string GetLocalizedValue(string key)
        {
            if (string.IsNullOrEmpty(key))
                return key;

            if(Application.isPlaying == false)
            {
                // ? Will force to update dictionary, if it was changed.
                if(lastLoadTime == null || lastLoadTime < DictionaryLastWriteTime)
                    FillDictionary();
                
                return GetValueFromDictionary(PreviewTextAssistant.PreviewLanguage, key);
            }
            else
                return GetValueFromDictionary(getSelectedLanguage(), key);
        }

        private static string GetValueFromDictionary(Language language, string key)
        {
            if (mainDictionary[language].TryGetValue(key, out string value) == false)
            {
                Debug.LogWarning($"Given key (\"{key}\") doesn't exist in the dictionary. Returned given key instead.");
                return key;
            }
            else
                return value;
        }
    }
}