using System.Collections.Generic;

namespace CGames
{
    public static class SaveLoadHelper
    {
        public static List<T> GetCorrectListFromSaveFile<T>(List<T> defaultList, List<T> loadedList)
        {
            if(loadedList.IsNullOrEmpty())
                return defaultList;
            
            if(loadedList.Count == defaultList.Count)
                return loadedList;

            List<T> adaptedList = new(defaultList);

            if(loadedList.Count < defaultList.Count)
            {
                for (int i = 0; i < loadedList.Count; i++)
                {
                    adaptedList[i] = loadedList[i];
                }
            }
            else
            {
                for (int i = 0; i < defaultList.Count; i++)
                {
                    adaptedList[i] = loadedList[i];
                }
            }

            return adaptedList;
        }   

        public static Dictionary<T, T1> GetCorrectDictionaryFromSaveFile<T, T1>(Dictionary<T, T1> defaultDictionary, Dictionary<T, T1> loadedDictionary)
        {
            if(loadedDictionary == null || loadedDictionary.Count == 0)
                return defaultDictionary;
            
            if(loadedDictionary.Count == defaultDictionary.Count)
                return loadedDictionary;
            
            Dictionary<T, T1> adaptedDictionary = new(defaultDictionary);

            if(loadedDictionary.Count < defaultDictionary.Count)
            {
                foreach (KeyValuePair<T, T1> kvp in loadedDictionary)
                {
                    adaptedDictionary[kvp.Key] = kvp.Value;                
                }
            }
            else
            {
                foreach (KeyValuePair<T, T1> kvp in defaultDictionary)
                {
                    if(loadedDictionary.ContainsKey(kvp.Key))
                        adaptedDictionary[kvp.Key] = loadedDictionary[kvp.Key];
                }
            }

            return adaptedDictionary;
        }
    }
}