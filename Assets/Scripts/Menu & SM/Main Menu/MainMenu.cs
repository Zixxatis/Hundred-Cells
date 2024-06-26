using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class MainMenu : MonoBehaviour, IGameModeEnteredNotifier, INewGameStartedNotifier
    {        
        [Header("Menu Panels")]
        [SerializeField] private LeaderboardsMP leaderboardsMP;
        [SerializeField] private StoreMP storeMP;
        [SerializeField] private TutorialsMP tutorialsMP;
        [SerializeField] private SettingsMP settingsMP;
        [SerializeField] private GameOverMP gameOverMP;

        [Header("Navigation Buttons - Main Menu")]
        [SerializeField] private Button startClassicButton;
        [SerializeField] private Button startInfiniteButton;
        [Space]
        [SerializeField] private Button continueButton;
        [SerializeField] private TextLocalizer continueButtonLTMP;
        [SerializeField] private GameObject continueClassicIconObject;
        [SerializeField] private GameObject continueInfiniteIconObject;
        [SerializeField] private GameObject startButtonsHolder;
        [Space]
        [SerializeField] private Button leaderBoardButton;
        [SerializeField] private Button storeButton;
        [SerializeField] private Button tutorialsButton;
        [SerializeField] private Button settingsButton;

        [Header("Various Elements")]
        [SerializeField] private GameObject mainMenuCanvasObject;
        [SerializeField] private GameObject mainMenuInfoObject;
        [Space]
        [SerializeField] private InGameNavigationBlock inGameNavigationBlock;

        public event Action OnNewGameStarted;
        public event Action OnGameModeEntered;

        private GameStatusHandler gameStatusHandler;
        private Func<uint> getCurrentScore;

        private Dictionary<Type, Action> openPanelActionsDictionary;
        
        [Inject]
        private void Construct(GameStatusHandler gameStatusHandler, ScoreCounter scoreCounter)
        {
            this.gameStatusHandler = gameStatusHandler;
            this.getCurrentScore = () => scoreCounter.CurrentScore;
        }

        private void Awake()
        {
            InitializeMenuPanels();
            InitializeButtons();

            OpenMenu();
        }

        private void InitializeMenuPanels()
        {
            gameStatusHandler.OnGameOver += OpenPanel<GameOverMP>;

            gameOverMP.Initialize(RestartSameMode);
            gameOverMP.OnSubMenuClosed += ValidatePlayButtons;

            openPanelActionsDictionary = new Dictionary<Type, Action>
            {
                { typeof(LeaderboardsMP), () => leaderboardsMP.OpenPanel() },
                { typeof(StoreMP), () => storeMP.OpenPanel() },
                { typeof(TutorialsMP), () => tutorialsMP.OpenPanel() },
                { typeof(SettingsMP), () => settingsMP.OpenPanel() },
                { typeof(GameOverMP), () => gameOverMP.OpenPanel() }
            };
        }

        private void InitializeButtons()
        {
            continueButton.onClick.AddListener(EnterPlayMode);
            startClassicButton.onClick.AddListener(StartClassicMode);
            startInfiniteButton.onClick.AddListener(StartInfiniteMode);

            leaderBoardButton.onClick.AddListener(OpenPanel<LeaderboardsMP>);
            storeButton.onClick.AddListener(OpenPanel<StoreMP>);     
            tutorialsButton.onClick.AddListener(OpenPanel<TutorialsMP>);
            settingsButton.onClick.AddListener(OpenPanel<SettingsMP>);
            
            inGameNavigationBlock.Initialize
            (
                OpenPanel<StoreMP>,
                OpenMenu
            );      
        }

        private void OpenMenu()
        {
            ValidatePlayButtons();

            mainMenuCanvasObject.ActivateObject();
            mainMenuInfoObject.ActivateObject();
            this.ActivateGameObject();
        }

        private void CloseMenu()
        {
            mainMenuCanvasObject.DeactivateObject();
            mainMenuInfoObject.DeactivateObject();
            this.DeactivateGameObject();
        }

        private void OpenPanel<T>() where T : MenuPanel
        {
            OpenMenu();

            Type type = typeof(T);

            if (openPanelActionsDictionary.TryGetValue(type, out var overrideAction))
                overrideAction.Invoke();
            else
                throw new InvalidOperationException($"Unsupported panel: {type}. Make sure this type is added to \"openPanelActionsDictionary\".");
        }

        private void StartClassicMode()
        {
            gameStatusHandler.ChangeGameMode(GameMode.Classic);
            OnNewGameStarted?.Invoke();
            EnterPlayMode();
        }

        private void StartInfiniteMode()
        {
            gameStatusHandler.ChangeGameMode(GameMode.Infinite);
            OnNewGameStarted?.Invoke();
            EnterPlayMode();
        }

        private void RestartSameMode()
        {
            OnNewGameStarted?.Invoke();
            EnterPlayMode();
        }

        private void EnterPlayMode()
        {
            OnGameModeEntered?.Invoke();
            CloseMenu();
        }

        private void ValidatePlayButtons()
        {
            if (getCurrentScore.Invoke() == 0)
            {
                continueButton.DeactivateGameObject();
                startButtonsHolder.ActivateObject();
            }
            else
            {
                continueButton.ActivateGameObject();
                startButtonsHolder.DeactivateObject();

                ChangeContinueButton();
            }
        }
        
        private void ChangeContinueButton()
        {
            switch (gameStatusHandler.GameMode)
            {
                case GameMode.Classic:
                    continueButtonLTMP.SetKeyAndUpdate("MM_ContinueClassic");
                    continueClassicIconObject.ActivateObject();
                    continueInfiniteIconObject.DeactivateObject();
                    break;

                case GameMode.Infinite:
                    continueButtonLTMP.SetKeyAndUpdate("MM_ContinueInfinite");
                    continueClassicIconObject.DeactivateObject();
                    continueInfiniteIconObject.ActivateObject();
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"Unsupported Game Mode: {gameStatusHandler.GameMode}. Make sure this type is added to \"ChangeContinueButtonText()\".");
            }
        }

        private void OnDestroy()
        {
            gameStatusHandler.OnGameOver -= OpenPanel<GameOverMP>;
            gameOverMP.OnSubMenuClosed -= ValidatePlayButtons;

            continueButton.onClick.RemoveListener(EnterPlayMode);
            startClassicButton.onClick.RemoveListener(StartClassicMode);
            startInfiniteButton.onClick.RemoveListener(StartInfiniteMode);
            
            leaderBoardButton.onClick.RemoveListener(OpenPanel<LeaderboardsMP>);
            storeButton.onClick.RemoveListener(OpenPanel<StoreMP>);
            tutorialsButton.onClick.RemoveListener(OpenPanel<TutorialsMP>);
            settingsButton.onClick.RemoveListener(OpenPanel<SettingsMP>);
        }
    }
}