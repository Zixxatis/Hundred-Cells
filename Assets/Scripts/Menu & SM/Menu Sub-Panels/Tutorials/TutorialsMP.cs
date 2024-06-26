using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class TutorialsMP : MenuPanel
    {
        [Header("Information Fields")]
        [SerializeField] private TextLocalizer headerLTMP;
        [SerializeField] private Image leftPreviewImage;
        [SerializeField] private Image rightPreviewImage;
        [SerializeField] private TextLocalizer descriptionLTMP;

        [Header("Buttons")]
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;

        private Func<TutorialTopicTheme, TutorialTopicSO> getTopicSO;
        private TutorialTopicSO DisplayedTopic => getTopicSO((TutorialTopicTheme)displayedTopicIndex);

        private int displayedTopicIndex;

        [Inject]
        private void Construct(ResourceSystem resourceSystem)
        {
            this.getTopicSO = resourceSystem.TutorialsRSS.GetTutorialTopicSO;
        }

        protected override void Awake()
        {
            base.Awake();

            previousButton.onClick.AddListener(ShowPreviousPage);
            nextButton.onClick.AddListener(ShowNextPage);
        }

        private void Start()
        {
            displayedTopicIndex = 0;

            UpdateInformation();
        }

        private void ShowPreviousPage()
        {
            displayedTopicIndex--;
            UpdateInformation();
        }

        private void ShowNextPage()
        {
            displayedTopicIndex++;
            UpdateInformation();
        }

        private void UpdateInformation()
        {
            TutorialTopicSO tutorialTopic = DisplayedTopic;

            headerLTMP.SetKeyAndUpdate(tutorialTopic.TitleLK);
            descriptionLTMP.SetKeyAndUpdate(tutorialTopic.DescriptionLK);

            HandleSpritesVisibility(tutorialTopic);
            HandleButtonsInteractivity();
        }

        private void HandleSpritesVisibility(TutorialTopicSO tutorialTopic)
        {
            if (tutorialTopic.LeftSprite != null)
            {
                leftPreviewImage.ActivateGameObject();
                leftPreviewImage.sprite = tutorialTopic.LeftSprite;
            }
            else
                leftPreviewImage.DeactivateGameObject();

            if (tutorialTopic.RightSprite != null)
            {
                rightPreviewImage.ActivateGameObject();
                rightPreviewImage.sprite = tutorialTopic.RightSprite;
            }
            else
                rightPreviewImage.DeactivateGameObject();
        }

        private void HandleButtonsInteractivity()
        {
            if (displayedTopicIndex == 0)
                previousButton.DisableInteractivityWithText();
            else
                previousButton.EnableInteractivityWithText();

            if(displayedTopicIndex == Enum<TutorialTopicTheme>.Length - 1)
                nextButton.DisableInteractivityWithText();
            else
                nextButton.EnableInteractivityWithText();
        }

        public override void PrepareBeforeOpening() { }
        public override void PrepareBeforeClosing() { }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();

            previousButton.onClick.RemoveListener(ShowPreviousPage);
            nextButton.onClick.RemoveListener(ShowNextPage);
        }
    }
}