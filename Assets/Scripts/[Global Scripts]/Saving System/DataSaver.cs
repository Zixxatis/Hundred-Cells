using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace CGames
{
    public class DataSaver<T> : ISaver<T> where T : Data, new()
    {
        private readonly List<ISavable<T>> savableScriptsList;

        public DataSaver(List<ISavable<T>> savableScriptsList)
        {
            this.savableScriptsList = savableScriptsList;
        }

        public void OverrideData(T data) => savableScriptsList.ForEach(x => x.ReceiveData(data));

        public void LoadData()
        {
            T data = LoadDataFromFile();
            OverrideData(data);
        }
        
        private T LoadDataFromFile()
        {
            T data = new();
            string FullPath = data.FullPath;
            string LogPrefix = data.LogPrefix;

            if (File.Exists(FullPath))
            {
                data = SaveFileHandler.Load<T>(FullPath);

                if (data != null)
                {
                    Debug.Log($"[{LogPrefix}] <color=#43BF0D>Loaded.</color>");
                    return data;
                }
                else
                    Debug.LogError($"[{LogPrefix}] Got a null data file.");
            }

            Debug.LogWarning($"[{LogPrefix}] No data file found. Creating a new one.");
            return new();
        }

        public void SaveData()
        {
            T data = new();
            savableScriptsList.ForEach(x => x.PassData(data));

            SaveDataToFile(data);
        }
        
        private void SaveDataToFile(T data)
        {
            SaveFileHandler.Save(data, data.FullPath);
            Debug.Log($"[{data.LogPrefix}] <color=#E67D12>Saved.</color>");
        }

        public async Task SaveDataAsync()
        {
            T data = new();
            savableScriptsList.ForEach(x => x.PassData(data));

            await SaveDataToFileAsync(data);
        }

        private async Task SaveDataToFileAsync(T data)
        {
            await SaveFileHandler.SaveAsync(data, data.FullPath);
            Debug.Log($"[{data.LogPrefix}] <color=#E67D12>Saved.</color>");
        }
    }
}