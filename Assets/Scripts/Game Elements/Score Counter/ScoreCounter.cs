using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

namespace CGames
{
    public class ScoreCounter : MonoBehaviour, ISavable<SessionData>
    {
        [Header("Counter - Elements")]
        [SerializeField] private GameObject personalBestObject;
        [Space]
        [SerializeField] private TextMeshProUGUI highScoreTMP;
        [SerializeField] private TextMeshProUGUI scoreTMP;

        [Header("Threshold Settings")]
        [SerializeField] private int scoreThreshold = 1000;

        public uint CurrentScore { get; private set; }

        private GameStatusHandler gameStatusHandler;
        private INewGameStartedNotifier newGameStartedNotifier;
        private Func<GameMode, uint> getPlayerRecord;
        private Action<int> gainCoinsAction;

        private int totalThresholdsOverflowCount;

        [Inject]
        private void Construct(LeaderboardSystem leaderboardSystem, GameStatusHandler gameStatusHandler, Wallet wallet, INewGameStartedNotifier newGameStartedNotifier)
        {
            this.gameStatusHandler = gameStatusHandler;
            this.newGameStartedNotifier = newGameStartedNotifier;

            this.getPlayerRecord = leaderboardSystem.GetPlayerRecord;
            this.gainCoinsAction = wallet.GainCoins;
        }

        public void ReceiveData(SessionData data)
        {
            this.CurrentScore = data.Score;
            this.totalThresholdsOverflowCount = (int)(data.Score / scoreThreshold);

            UpdateScoreText();
            UpdateHighScoreText();
        }

        public void PassData(SessionData data)
        {
            data.Score = this.CurrentScore;
        }

        private void Awake()
        {
            gameStatusHandler.OnGameOver += ResetScore;
            newGameStartedNotifier.OnNewGameStarted += UpdateHighScoreText;         
        }

        public void GivePointsForPlacingCell(int cellsAmount)
        {
            uint scored = (uint)cellsAmount * GameConstants.PointsForCell;
            UpdatePlayerScore(scored);
        }

        public void AddPointsForClearedCells(List<byte> multipliersList)
        {
            uint scored = 0;
            multipliersList.ForEach(x => scored += x * GameConstants.PointsForCell);

            UpdatePlayerScore(scored);
        }

        private void UpdatePlayerScore(uint gainedScore)
        {
            CurrentScore += gainedScore;
            UpdateScoreText();

            HandleThresholdCounter();
        }

        private void HandleThresholdCounter()
        {
            int newThresholdsOverflow = (int)(CurrentScore / scoreThreshold);

            while (newThresholdsOverflow > totalThresholdsOverflowCount)
            {
                totalThresholdsOverflowCount ++;
                gainCoinsAction.Invoke(1);
            }
        }

        private void ResetScore()
        {
            CurrentScore = 0;
            totalThresholdsOverflowCount = 0;

            UpdateScoreText();
        }

        private void UpdateScoreText() => scoreTMP.text = CurrentScore.ToString();
        private void UpdateHighScoreText()
        {
            if(getPlayerRecord.Invoke(gameStatusHandler.GameMode) != 0)
            {
                personalBestObject.ActivateObject();
                highScoreTMP.text = getPlayerRecord.Invoke(gameStatusHandler.GameMode).ToString();
            }
            else
                personalBestObject.DeactivateObject();
        }

        private void OnDestroy()
        {
            gameStatusHandler.OnGameOver -= ResetScore;
            newGameStartedNotifier.OnNewGameStarted -= UpdateHighScoreText;  
        }
    }
}