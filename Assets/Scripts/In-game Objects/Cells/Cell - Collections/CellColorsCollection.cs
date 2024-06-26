using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CGames
{
    [CreateAssetMenu(fileName = "Cell Colors Collection", menuName = "Scriptable Objects/Cell Colors Collection", order = 0)]
    public class CellColorsCollection : ScriptableObject, ILocalizable
    {
        [field: Header("Main")]
        [field: SerializeField] public ColorsCollectionType ColorsCollectionType { get; private set; }
        [field: SerializeField] public LocalizationKeyField TitleLK { get; private set; }
        [field: SerializeField] public int Price { get; private set; }

        [field: Header("Theme Colors")]
        [field: SerializeField] public bool UsesLightTheme { get; private set; }
        [field: Space]
        [field: SerializeField] public Color32 BackgroundColor { get; private set; }
        [field: SerializeField] public Color32 BackgroundPatternColor { get; private set; }
        
        [Header("Cell Colors")]
        [SerializeField] private List<CellColorPair> cellColorsPairList;

        public Color32 GetColor32ForCell(CellColor cellColor) => cellColorsPairList.First(x => x.CellColor == cellColor).Color32;

        List<LocalizationKeyField> ILocalizable.GetAllLocalizationKeys()
        {
            return new()
            {
                TitleLK
            };
        }

    #if UNITY_EDITOR
    #region Object Creation & Validation
        private void OnValidate()
        {
            if (cellColorsPairList.IsNullOrEmpty())
            {
                CreateNewList();
                return;
            }

            List<CellColor> enumValuesList = Enum.GetValues(typeof(CellColor)).Cast<CellColor>().ToList();

            AdjustColorList(enumValuesList);
            CheckForDuplicates(enumValuesList);

            cellColorsPairList.Sort((x, y) => x.CellColor.CompareTo(y.CellColor));
        }

        private void CreateNewList()
        {   
            List<CellColor> enumValuesList = Enum.GetValues(typeof(CellColor)).Cast<CellColor>().ToList();
            cellColorsPairList = enumValuesList.Select(x => new CellColorPair(x, new(0, 0, 0, 255))).ToList();
        }

        private void AdjustColorList(List<CellColor> enumValuesList)
        {
            if(cellColorsPairList.Count == Enum<CellColor>.Length)
                return;

            if (cellColorsPairList.Count > Enum<CellColor>.Length)
                RemoveExtraColor();
            else
                AddMissingColor(enumValuesList);
        }

        private void RemoveExtraColor()
        {
            int enumCount = Enum<CellColor>.Length;
            cellColorsPairList.RemoveRange(enumCount, cellColorsPairList.Count - enumCount);

            Debug.LogWarning("You've tried to add an extra color. That's not allowed.");
        }

        private void AddMissingColor(List<CellColor> enumValuesList)
        {
            cellColorsPairList.AddRange(enumValuesList.Where(x => cellColorsPairList.All(y => y.CellColor != x))
                                                      .Select(x => new CellColorPair(x, new Color32(0, 0, 0, 255))));
                        
            Debug.LogWarning("You've tried to remove a color. That's not allowed.");
        }

        private void CheckForDuplicates(List<CellColor> enumValuesList)
        {
            IEnumerable<CellColor> currentCellColorsList = cellColorsPairList.Select(x => x.CellColor).Distinct();

            if (enumValuesList.Except(currentCellColorsList).ToList().Any())
                Debug.LogWarning("List contains duplicates and missing values!");
        }
    #endregion
    #endif

    }
}