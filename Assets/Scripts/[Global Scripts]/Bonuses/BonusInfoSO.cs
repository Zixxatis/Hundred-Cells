using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    [CreateAssetMenu(fileName = "New Bonus Info", menuName = "Scriptable Objects/Bonus Info SO", order = 0)]
    public class BonusInfoSO : ScriptableObject, ILocalizable
    {
        [field: Header("General")]
        [field: SerializeField] public BonusType BonusType { get; private set; }
        [field: SerializeField] public LocalizationKeyField TitleLK { get; private set; }
        [field: SerializeField] public CellColor MatchingCellColor { get; private set; }

        [field: Header("Helper Info")]
        [field: SerializeField] public bool ShouldShowBonusHelperInfo { get; private set; } = true;
        [field: SerializeField] public LocalizationKeyField BonusHelperInfoLK { get; private set; }

        [field: Header("Economics")]
        [field: SerializeField, Min(1)] public int CellsClearedTarget { get; private set; }
        [field: SerializeField, Min(1)] public int CoinsPrice { get; private set; }
        
        [field: Header("UI/UX")]
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public AudioClip EffectAudioClip { get; private set; }

        public float GetProgressFill(int clearedCellsAmount) => clearedCellsAmount / (float)CellsClearedTarget;

        List<LocalizationKeyField> ILocalizable.GetAllLocalizationKeys()
        {
            return new()
            {
                TitleLK,
                BonusHelperInfoLK
            };
        }
    }
}