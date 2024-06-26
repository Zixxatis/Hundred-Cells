using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class AdvertisementCanvas : MonoBehaviour
    {
        [Header("Banner")]
        [SerializeField] private Button bannerButton;
        [SerializeField] private GameObject bannerObject;

        [Header("Pop-up")]
        [SerializeField] private GameObject popupBlockerObject;
        [SerializeField] private Button popupButton;
        [SerializeField] private GameObject popupObject;
        [Space]
        [SerializeField] private Button closeButton;

        private AdvertisementHandler advertisementHandler;

        [Inject]
        private void Construct(AdvertisementHandler advertisementHandler)
        {
            this.advertisementHandler = advertisementHandler;
        }

        private void Awake()
        {
            bannerButton.onClick.AddListener(OpenLink);
            popupButton.onClick.AddListener(OpenLink);
            closeButton.onClick.AddListener(ClosePopup);

            advertisementHandler.OnAdvertisementsRemoved += HideBanner;
        }

        private void Start()
        {
            if(advertisementHandler.IsAdvertisementsEnabled == false)
                HideBanner();

            ClosePopup();
        }

        public void TryToOpenPopup()
        {
            if(advertisementHandler.IsAdvertisementsEnabled)
            {
                popupObject.ActivateObject();
                popupBlockerObject.ActivateObject();
            }
        }

        private void ClosePopup()
        {
            popupObject.DeactivateObject();
            popupBlockerObject.DeactivateObject();
        }

        private void HideBanner() => bannerObject.DeactivateObject();

        private void OpenLink() => Application.OpenURL("https://zixxatis.itch.io/loot-lagoon-plunder");

        private void OnDestroy()
        {
            bannerButton.onClick.RemoveListener(OpenLink);
            popupButton.onClick.RemoveListener(OpenLink);
            closeButton.onClick.RemoveListener(ClosePopup);

            advertisementHandler.OnAdvertisementsRemoved -= HideBanner;
        }
    }
}