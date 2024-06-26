using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    [CreateAssetMenu(fileName = "New Tutorial Topic", menuName = "Scriptable Objects/Tutorial Topic SO", order = 0)]
    public class TutorialTopicSO : ScriptableObject, ILocalizable
    {
        [field: SerializeField] public TutorialTopicTheme TutorialTopicTheme { get; private set; }
        [field: Space]
        [field: SerializeField] public LocalizationKeyField TitleLK { get; private set; }
        [field: SerializeField] public LocalizationKeyField DescriptionLK { get; private set; }
        [field: Space]
        [field: SerializeField] public Sprite LeftSprite { get; private set; }
        [field: SerializeField] public Sprite RightSprite { get; private set; }

        public List<LocalizationKeyField> GetAllLocalizationKeys()
        {
            List<LocalizationKeyField> localizationKeysList = new()
            {
                TitleLK, DescriptionLK
            };

            return localizationKeysList;
        }

        [ContextMenu("Fill-in to match To Selected Theme")]
        private void AdjustToSelectedTheme()
        {
            TitleLK.UpdateKey($"Tutorials_{TutorialTopicTheme}");
            DescriptionLK.UpdateKey($"TutorialsDesc_{TutorialTopicTheme}");
        }
    }
}