using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CGames
{
    public static class SaveFileHandler
    {
        /// <summary> Generic method for loading file of T type. </summary>
        /// <returns> Data of T type, if it was found and can be parsed. Otherwise returns default of T. </returns>
        public static T Load<T>(string fileFullPath)
        {
            if (File.Exists(fileFullPath))
            {
                try
                { 
                    using FileStream fileStream = new(fileFullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    using StreamReader reader = new(fileStream);

                    string dataToRead = reader.ReadToEnd();
                    dataToRead = DataEncryption.Decrypt(dataToRead);

                    return JsonConvert.DeserializeObject<T>(dataToRead);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Error occurred when trying to load data from file: {fileFullPath}.");
                    Debug.LogError($"Error: {exception}");
                }
            }

            return default;
        }

        /// <summary> Generic method for saving file of T type. </summary>
        public static void Save<T>(T data, string fileFullPath)
        {
            try
            {   
                string dataToWrite = JsonConvert.SerializeObject(data, Formatting.Indented, new StringEnumConverter());
                dataToWrite = DataEncryption.Encrypt(dataToWrite);

                using FileStream stream = new(fileFullPath, FileMode.Create, FileAccess.Write, FileShare.None);
                using StreamWriter writer = new(stream);

                writer.Write(dataToWrite);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error occurred, when trying to save data to file: {fileFullPath}.");
                Debug.LogError($"Error: {exception}");
            }
        }

        /// <summary> Generic async method for saving file of T type. </summary>
        public static async Task SaveAsync<T>(T data, string fileFullPath)
        {
            try
            {
                string dataToWrite = JsonConvert.SerializeObject(data, Formatting.Indented, new StringEnumConverter());
                dataToWrite = DataEncryption.Encrypt(dataToWrite);

                using FileStream stream = new(fileFullPath, FileMode.Create, FileAccess.Write, FileShare.None);
                using StreamWriter writer = new(stream);

                await writer.WriteAsync(dataToWrite);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error occurred, when trying to save data to file: {fileFullPath}.");
                Debug.LogError($"Error: {exception}");
            }
        }
    }
}