using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class LeaderboardsMP : MenuPanel
    {
        [SerializeField] private Button swapModeButton;
        [Space]
        [SerializeField] private Transform leaderBoardsHolderTransform;
        [SerializeField] private List<LeaderboardObject> leaderboardObjectsList;
        
        private GameMode displayedGameMode;
        private Func<GameMode, List<LeaderboardRecord>> getLeaderboardRecords;

        [Inject]
        private void Construct(LeaderboardSystem leaderboardSystem)
        {
            this.getLeaderboardRecords = leaderboardSystem.GetLeaderboardsRecords;
        }

        protected override void Awake()
        {
            base.Awake();

            swapModeButton.onClick.AddListener(SwapMode);
            InitializeLeaderboards();
        }

        private void InitializeLeaderboards()
        {
            List<GameMode> gameModes = Enum.GetValues(typeof(GameMode)).Cast<GameMode>().ToList();

            for (int i = 0; i < gameModes.Count; i++)
            {
                GameMode gameMode = gameModes[i];
                leaderboardObjectsList[i].Initialize(gameMode, () => getLeaderboardRecords.Invoke(gameMode));
            }
        }
        
        public override void PrepareBeforeOpening()
        {
            leaderboardObjectsList.ForEach(x => x.UpdateLeaderboard());

            OpenLeaderboardForDisplayedGameMode();
        }

        private void SwapMode()
        {
            int currentModeIndex = (int)displayedGameMode;
            currentModeIndex++;

            if(currentModeIndex == Enum<GameMode>.Length)
                currentModeIndex = 0;

            displayedGameMode = (GameMode)currentModeIndex;

            OpenLeaderboardForDisplayedGameMode();
        }

        private void OpenLeaderboardForDisplayedGameMode()
        {
            foreach (LeaderboardObject leaderboard in leaderboardObjectsList)
            {
                if(leaderboard.GameMode == displayedGameMode)
                {
                    leaderboard.ActivateGameObject();
                    leaderboard.ResetPosition();
                }
                else
                    leaderboard.DeactivateGameObject();
            }
        }

        public override void PrepareBeforeClosing() => displayedGameMode = GameMode.Classic;

        protected override void OnDestroy()
        {
            base.OnDestroy();

            swapModeButton.onClick.RemoveListener(SwapMode);
        }

        #if UNITY_EDITOR
        [ContextMenu("Fill List")]
        private void FillList()
        {   
            if(leaderBoardsHolderTransform == null)
                return;

            leaderboardObjectsList = new();

            foreach (Transform child in leaderBoardsHolderTransform)
            {
                if(child.TryGetComponent(out LeaderboardObject recordObject))
                    leaderboardObjectsList.Add(recordObject);
            }
            
            if(leaderboardObjectsList.Count != Enum<GameMode>.Length)
                Debug.LogWarning($"List doesn't match expected number of elements! ({leaderboardObjectsList.Count} / {Enum<GameMode>.Length})");
            else
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
        #endif
    }
}