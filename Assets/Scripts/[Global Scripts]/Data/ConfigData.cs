using System;

namespace CGames
{
    [Serializable]
    public class ConfigData : Data
    {
        public override byte Version => 1;
        public override string LogPrefix => "CONFIG"; 
        protected override string FileName => "config.data";
        
        // Global
        public byte SaveVersion { get; set; }

        // Player Name
        public string PlayerName { get; set; }

        // Sound Settings
        public float MusicVolume { get; set; }
        public float EffectsVolume { get; set; }

        // Locale Settings
        public Language Language { get; set; }

        // Preferences
        public DraggingDistance DraggingDistance { get; set; }

        public ConfigData()
        {
            SaveVersion = Version;

            PlayerName = string.Empty;

            MusicVolume = 0.4f;
            EffectsVolume = 0.6f;

            Language = GetSystemLanguage();

            DraggingDistance = DraggingDistance.Medium;
        }

        private Language GetSystemLanguage()
        {
            string languageCode = System.Globalization.CultureInfo.CurrentCulture.ToString()[..2];

            return languageCode switch
            {
                "ru" or "be" or "hy" or "ky" or "kk" or "ua" => Language.Russian,
                _ => Language.English
            };
        }
    }
}