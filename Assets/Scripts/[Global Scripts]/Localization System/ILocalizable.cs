using System.Linq;
using System.Collections.Generic;

namespace CGames
{
    public interface ILocalizable
    {
        public abstract List<LocalizationKeyField> GetAllLocalizationKeys();
        
        public bool HasLocalizationKey(string key)
        {
            return GetAllLocalizationKeys().Exists(x => x.LocalizationKey == key);
        }

        public void ReplaceKey(string oldKey, string newKey)
        {
            List<LocalizationKeyField> localizationKeyFields = GetAllLocalizationKeys().Where(x => x.LocalizationKey == oldKey).ToList();

            localizationKeyFields.ForEach(x => x.UpdateKey(newKey));
        }
    }
}