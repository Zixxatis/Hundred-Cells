using UnityEngine;

namespace CGames
{
    [CreateAssetMenu(fileName = "New Language Info", menuName = "Scriptable Objects/Language Info SO", order = 0)]
    public class LanguageInfoSO : ScriptableObject
    {
        private const float FlagRatio = 3f / 2f;

        [field: SerializeField] public Language Language { get; private set; }
        [field: Space]
        [field: SerializeField] public Sprite FlagSprite { get; private set; }
        [Space]
        [SerializeField] private float rateToUSD;
        [SerializeField] private bool shouldRoundPrice;

        public float GetPrice(float priceInUSD)
        {
            if(shouldRoundPrice)
                return Mathf.RoundToInt(priceInUSD * rateToUSD);
            else
                return priceInUSD * rateToUSD;
        }

        private void OnValidate()
        {
            if(FlagSprite.bounds.size.x / FlagSprite.bounds.size.y != FlagRatio)
                Debug.LogWarning($"Selected sprite doesn't match 3:2 ratio. This sprite may be displayed incorrectly.");
        }
    }
}