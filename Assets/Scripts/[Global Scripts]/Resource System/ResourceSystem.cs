using System.Linq;
using UnityEngine;

namespace CGames
{
    public class ResourceSystem
    {
        public BonusesRSS BonusesRSS { get; private set; }
        public BundlesRSS BundlesRSS { get; private set; }
        public ColorsRSS ColorsRSS { get; private set; }
        public LocalizationRSS LocalizationRSS { get; private set; }
        public TutorialsRSS TutorialsRSS { get; private set; }
        public ShapesRSS ShapesRSS { get; private set; }

        public ResourceSystem()
        {
            BonusesRSS = new(Resources.LoadAll<BonusInfoSO>("Bonus Info").ToList());
            BundlesRSS = new(Resources.LoadAll<CoinsBundleSO>("Coin Bundles").ToList());
            ColorsRSS = new(Resources.LoadAll<CellColorsCollection>("Cell Colors Collections").ToList(), Resources.Load<CoreThemeColorsConfig>("Configs/Core Theme Colors Config"));
            LocalizationRSS = new(Resources.LoadAll<LanguageInfoSO>("Localization Info").ToList());
            TutorialsRSS = new(Resources.LoadAll<TutorialTopicSO>("Tutorial Topics").ToList());
            ShapesRSS = new(Resources.LoadAll<Shape>("Shapes").ToList(), Resources.Load<DraggingDistanceConfig>("Configs/Dragging Distance Config"));
        }        
    }
}