using System.Collections.Generic;
using System.Linq;

namespace CGames
{
    public class LocalizationRSS
    {
        private readonly List<LanguageInfoSO> languageInfoList;

        public LocalizationRSS(List<LanguageInfoSO> LanguageInfoList)
        {
            this.languageInfoList = LanguageInfoList;
            LanguageInfoList.Sort((x, y) => x.Language.CompareTo(y.Language));
        }

        public LanguageInfoSO GetLanguageInfoSO(Language language)
        {
            return languageInfoList.First(x => x.Language == language);
        }

        public LanguageInfoSO GetLanguageInfoSO(int languageIndex) => GetLanguageInfoSO((Language)languageIndex);
    }
}