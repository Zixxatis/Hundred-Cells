using System;
using System.Collections.Generic;
using System.Linq;

namespace CGames
{
    public static class LeaderboardsUtilities
    {
        public const byte LeaderboardPlayerCapacity = 10;

        private const uint ScorePerLeaderboardSpotForClassic = 10000;
        private const uint ScorePerLeaderboardSpotForInfinite = 15000;

        private static readonly List<string> defaultNamesList = new()
        {
            "Aaron", "Bob", "Billy", "Boris", "Clement", "George", "Jim", "Joe", "Steve", "Walter"
        };

        public static Dictionary<GameMode, uint> GetCorrectPlayerRecordsDictionary(Dictionary<GameMode, uint> loadedDictionary)
        {
            Dictionary<GameMode, uint> defaultDictionary = Enum.GetValues(typeof(GameMode)).Cast<GameMode>()
                                                               .ToDictionary(x => x, x => (uint)0);

            return SavesHelper.GetCorrectDictionaryFromSaveFile(defaultDictionary, loadedDictionary);
        }

        public static Dictionary<GameMode, List<LeaderboardRecord>> GetCorrectLeaderboardsDictionary(Dictionary<GameMode, List<LeaderboardRecord>> loadedDictionary)
        {
            Dictionary<GameMode, List<LeaderboardRecord>> defaultDictionary = Enum.GetValues(typeof(GameMode)).Cast<GameMode>()
                                                                                  .ToDictionary(x => x, x => GetBaseLeaderboardsList(x));

            return SavesHelper.GetCorrectDictionaryFromSaveFile(defaultDictionary, loadedDictionary);
        }

        private static List<LeaderboardRecord> GetBaseLeaderboardsList(GameMode gameMode)
        {
            List<string> namesList = new(defaultNamesList);
            namesList.Shuffle();

            uint scorePerSpot = gameMode switch
            {
                GameMode.Classic => ScorePerLeaderboardSpotForClassic,
                GameMode.Infinite => ScorePerLeaderboardSpotForInfinite,
                _ => throw new ArgumentOutOfRangeException($"Game mode {nameof(gameMode)} has no default leaderboard records list for it.")
            };

            List<LeaderboardRecord> leaderboardRecords = new(namesList.Count);
            for (int i = 0; i < namesList.Count; i++)
            {
                leaderboardRecords.Add(new LeaderboardRecord(namesList[i], (uint)(i + 1) * scorePerSpot));
            }

            return leaderboardRecords.OrderByDescending(x => x.Score).ToList();
        }
    }
}