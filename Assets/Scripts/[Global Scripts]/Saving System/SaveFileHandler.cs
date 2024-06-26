using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CGames
{
    /// <summary> Class that works with reading/writing files. </summary>
    public static class SaveFileHandler
    {
        /// <summary> Generic method for loading file of T type. </summary>
        /// <returns> Data of T type, if it was found. Otherwise returns default of T (null). </returns>
        public static T Load<T>(string fileFullPath)
        {
            if (File.Exists(fileFullPath))
            {
                try
                { 
                    string dataToLoad = "";

                    using FileStream fileStream = new(fileFullPath, FileMode.Open);
                    using StreamReader reader = new(fileStream);

                    dataToLoad = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(dataToLoad);
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

                using FileStream stream = new(fileFullPath, FileMode.Create);
                using StreamWriter writer = new(stream);

                writer.Write(dataToWrite);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error occurred, when trying to save data to file: {fileFullPath}.");
                Debug.LogError($"Error: {exception}");
            }
        }
    }
}