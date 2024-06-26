using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CGames
{
    public class ColorsRSS
    {
        private readonly List<CellColorsCollection> cellColorsCollectionsList;
        private readonly Dictionary<bool, Color> graphicThemeColorsDictionary;

        public ColorsRSS(List<CellColorsCollection> cellColorsCollectionsList, CoreThemeColorsConfig coreThemeColorsConfig)
        {
            this.cellColorsCollectionsList = cellColorsCollectionsList;
            cellColorsCollectionsList.Sort((x, y) => x.ColorsCollectionType.CompareTo(y.ColorsCollectionType));

            graphicThemeColorsDictionary = coreThemeColorsConfig.GetThemeColorsDictionary();
        }

        public CellColorsCollection GetCellColorsCollection(ColorsCollectionType colorsCollectionType)
        {
            return cellColorsCollectionsList.First(x => x.ColorsCollectionType == colorsCollectionType);
        }

        public Color GetColorForGraphicElement(bool isForLightTheme) => graphicThemeColorsDictionary[isForLightTheme];
    }
}