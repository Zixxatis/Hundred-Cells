using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    [CreateAssetMenu(fileName = "New Coins Bundle", menuName = "Scriptable Objects/Coins bundle SO", order = 0)]
    public class CoinsBundleSO : ScriptableObject, ILocalizable
    {
        [field: SerializeField] public LocalizationKeyField TitleLK { get; private set; }
        [field: Space(10)]
        [field: SerializeField] public BundleRarity BundleRarity { get; private set; }
        [field: SerializeField] public Color32 BackgroundColor { get; private set; }
        [field: Space(10)]
        [field: SerializeField, Min(0)] public int CoinsAmount { get; private set; }
        [field: SerializeField, Min(0)] public int BonusAmount { get; private set; }
        [field: SerializeField] public bool IsDisablingAds { get; private set; }
        [field: Space(10)]
        [field: SerializeField] public float PriceInUSD { get; private set; }

        public bool HasBonus => BonusAmount > 0;
        public int IncreasedBonusValue => BonusAmount * 2;

        List<LocalizationKeyField> ILocalizable.GetAllLocalizationKeys()
        {
            return new()
            {
                TitleLK
            };
        }
    }
}