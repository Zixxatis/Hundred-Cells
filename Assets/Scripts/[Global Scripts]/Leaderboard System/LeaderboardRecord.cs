using System;

namespace CGames
{
    [Serializable]
    public class LeaderboardRecord
    {
        public string PlayerName { get; private set; }
        public uint Score { get; private set; }
        
        public LeaderboardRecord(string playerName, uint score)
        {
            PlayerName = playerName;
            Score = score;
        }
    }
}