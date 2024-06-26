using TMPro;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class PlayerNameSS : SettingSlot
    {
        [Header("Settings - Player Name")]
        [SerializeField] private TMP_InputField playerNameInputField;

        private LeaderboardSystem leaderboardSystem;

        [Inject]
        private void Construct(LeaderboardSystem leaderboardSystem)
        {
            this.leaderboardSystem = leaderboardSystem;
        }

        public override void PrepareSettingSlot()
        {
            playerNameInputField.onSelect.AddListener((x) => playerNameInputField.text = "");
            playerNameInputField.onDeselect.AddListener(leaderboardSystem.ChangePlayerName);
            playerNameInputField.onSubmit.AddListener(leaderboardSystem.ChangePlayerName);
        }

        public override void MatchValuesToCurrent()
        {
            playerNameInputField.text = leaderboardSystem.PlayerName;
        }

        private void OnDestroy()
        {        
            playerNameInputField.onSelect.RemoveAllListeners();
            playerNameInputField.onDeselect.RemoveListener(leaderboardSystem.ChangePlayerName);
            playerNameInputField.onSubmit.RemoveListener(leaderboardSystem.ChangePlayerName);
        }
    }
}