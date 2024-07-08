using System.IO;
using System.Text;
using UnityEngine;

namespace CGames
{
    public static class DataEncryption
    {
        private static readonly string configFileName = "Encryption Config";
        private static readonly string configFolder = "Configs";

        private static string PathInAssets => Path.Combine(configFolder, configFileName);
        private static string GlobalPath => Path.Combine(Application.dataPath, configFolder);

        private static EncryptionConfig EncryptionConfig
        {
            get
            {
                if(encryptionConfig == null)
                    encryptionConfig = Resources.Load<EncryptionConfig>(PathInAssets);

                if(encryptionConfig == null)
                    Debug.LogError($"Encryption Config not found! All files won't be encrypted and decrypted.\nPlease, make sure that the file \"{configFileName}\" exists in {GlobalPath}.");
                
                return encryptionConfig;
            }
        }

        private static EncryptionConfig encryptionConfig;

        public static string Encrypt(string data)
        {
            if(EncryptionConfig == null || EncryptionConfig.ShouldEncrypt == false)
                return data;
            else
                return CryptographicConvert(data);
        }

        public static string Decrypt(string data)
        {
            if(EncryptionConfig == null || EncryptionConfig.ShouldDecrypt == false)
                return data;
            else
                return CryptographicConvert(data);
        }

        private static string CryptographicConvert(string data)
        {
            string codeWord = EncryptionConfig.CodeWord;
            StringBuilder modifiedData = new();
            
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData.Append((char)(data[i] ^ codeWord[i % codeWord.Length]));
            }

            return modifiedData.ToString();
        }
    }
}