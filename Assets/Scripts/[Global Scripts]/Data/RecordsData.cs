using System;
using System.Collections.Generic;

namespace CGames
{
    [Serializable]
    public class RecordsData : Data
    {
        public override byte Version => 1;
        public override string LogPrefix => "RECORDS";
        protected override string FileName => "records.data";

        // Global
        public byte SaveVersion { get; set; }

        // Records
        public Dictionary<GameMode, uint> PlayerRecordsDictionary { get; set; }
        public Dictionary<GameMode, List<LeaderboardRecord>> LeaderboardsDictionary { get; set; }

        public RecordsData()
        {        
            SaveVersion = Version;

            PlayerRecordsDictionary = new();
            LeaderboardsDictionary = new();
        }
    }
}