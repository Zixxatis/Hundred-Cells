using System.Collections.Generic;
using System.Linq;

namespace CGames
{
    public class LeaderboardSystem : ISavable<RecordsData>, ISavable<ConfigData>
    {
        public string PlayerName { get; private set; }
        private Dictionary<GameMode, uint> playerRecordsDictionary;
        private Dictionary<GameMode, List<LeaderboardRecord>> leaderboardsDictionary;

        void ISavable<RecordsData>.ReceiveData(RecordsData data)
        {
            playerRecordsDictionary = LeaderboardsUtilities.GetCorrectPlayerRecordsDictionary(data.PlayerRecordsDictionary);
            leaderboardsDictionary = LeaderboardsUtilities.GetCorrectLeaderboardsDictionary(data.LeaderboardsDictionary);
        }

        void ISavable<RecordsData>.PassData(RecordsData data)
        {
            data.PlayerRecordsDictionary = playerRecordsDictionary;
            data.LeaderboardsDictionary = leaderboardsDictionary;
        }

        void ISavable<ConfigData>.ReceiveData(ConfigData data)
        {
            PlayerName = data.PlayerName;

            if(string.IsNullOrEmpty(PlayerName))
                PlayerName = LocalizationDictionary.GetLocalizedValue("Settings_DefaultName");
        }

        void ISavable<ConfigData>.PassData(ConfigData data)
        {
            data.PlayerName = PlayerName;
        }

        public void ChangePlayerName(string newName)
        {
            if(string.IsNullOrEmpty(newName) == false)
                PlayerName = newName;
        }

        public uint GetPlayerRecord(GameMode gameMode) => playerRecordsDictionary[gameMode];
        public List<LeaderboardRecord> GetLeaderboardsRecords(GameMode gameMode) => leaderboardsDictionary[gameMode];

        /// <summary> Will check and update LeaderboardData if player record exceeded any of them. Gives OUT a type of leaderboard update. </summary>
        /// <returns> Returns true if personal record or leaderboards were changed. </returns>
        public bool TryToUpdateLeaderboards(uint finalScore, GameMode gameMode, out LeaderboardUpdateType leaderboardUpdateType)
        {
            bool isUpdated = false;
            leaderboardUpdateType = LeaderboardUpdateType.None;

            if(playerRecordsDictionary[gameMode] < finalScore)
            {
                playerRecordsDictionary[gameMode] = finalScore;

                leaderboardUpdateType = LeaderboardUpdateType.PersonalBest;
                isUpdated = true;
            }
            
            List<LeaderboardRecord> currentModeLeaderboard = leaderboardsDictionary[gameMode];

            if(currentModeLeaderboard[^1].Score < finalScore)
            {
                currentModeLeaderboard.Add(new(PlayerName, finalScore));
                currentModeLeaderboard.Sort((x, y) => y.Score.CompareTo(x.Score));
                currentModeLeaderboard.Remove(currentModeLeaderboard[^1]);

                if(leaderboardUpdateType != LeaderboardUpdateType.None)
                    leaderboardUpdateType = LeaderboardUpdateType.Both;
                else
                    leaderboardUpdateType = LeaderboardUpdateType.LeaderboardRecord;

                isUpdated = true;
            }

            return isUpdated;
        }
    }
}