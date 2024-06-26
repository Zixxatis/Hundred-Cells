using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CGames
{
    public class StoreMP : MenuPanel
    {
        [Header("Panels")]
        [SerializeField] private BonusesStorePanel bonusesStorePanel;
        [SerializeField] private VisualsStorePanel visualsStorePanel;
        [SerializeField] private BundlesStorePanel bundlesStorePanel;

        [Header("Buttons")]
        [SerializeField] private Button bonusesPanelButton;
        [SerializeField] private Button visualsPanelButton;
        [SerializeField] private Button bundlesPanelButton;

        private Dictionary<StorePanel, Button> panelsAndButtonsDictionary;
        private StorePanel openedStorePanel;

        protected override void Awake()
        {
            base.Awake();

            panelsAndButtonsDictionary = new()
            {
                { bonusesStorePanel, bonusesPanelButton },
                { visualsStorePanel, visualsPanelButton },
                { bundlesStorePanel, bundlesPanelButton }
            };

            bonusesPanelButton.onClick.AddListener(() => OpenStorePanel(bonusesStorePanel));
            visualsPanelButton.onClick.AddListener(() => OpenStorePanel(visualsStorePanel));
            bundlesPanelButton.onClick.AddListener(() => OpenStorePanel(bundlesStorePanel));
        }

        public override void PrepareBeforeOpening()
        {
            if(openedStorePanel == null)
            {
                panelsAndButtonsDictionary.Keys.ToList().ForEach(x => x.HidePanel());
                OpenStorePanel(bonusesStorePanel);
            }
            else
                OpenStorePanel(openedStorePanel);
        }

        private void OpenStorePanel(StorePanel storePanelToDisplay)
        {
            if(openedStorePanel != null)
            {
                openedStorePanel.HidePanel();
                panelsAndButtonsDictionary[openedStorePanel].interactable = true;
            }

            storePanelToDisplay.DisplayPanel();
            panelsAndButtonsDictionary[storePanelToDisplay].interactable = false;

            openedStorePanel = storePanelToDisplay;
        }

        public override void PrepareBeforeClosing()
        {
            openedStorePanel.HidePanel();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        
            bonusesPanelButton.onClick.RemoveAllListeners();
            visualsPanelButton.onClick.RemoveAllListeners();
            bundlesPanelButton.onClick.RemoveAllListeners();
        }
    }
}