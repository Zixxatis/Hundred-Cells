using System;
using System.Collections.Generic;

namespace CGames
{
    [Serializable]
    public class SessionData : Data
    {
        public override byte Version => 1;
        public override string LogPrefix => "GAME";
        protected override string FileName => "session.data";

        public byte SaveVersion { get; set; }

        public GameMode GameMode { get; set; }
        public uint Score { get; set; }
        public List<ShapeData> ShapesInHandDataList { get; set; }
        public List<CellColor> CellsColorData { get; set; }
        public bool CanUseBonusesThisTurn { get; set; }

        public SessionData()
        {        
            SaveVersion = Version;

            GameMode = GameMode.Classic;
            Score = 0;
            ShapesInHandDataList = new();
            CellsColorData = new();
            CanUseBonusesThisTurn = false;
        }

        public SessionData(GameMode gameMode, uint score, List<ShapeData> shapesInHandDataList, List<CellColor> cellsColorData, bool canUseBonusesThisTurn)
        {
            SaveVersion = Version;

            GameMode = gameMode;
            Score = score;
            ShapesInHandDataList = shapesInHandDataList;
            CellsColorData = cellsColorData;

            CanUseBonusesThisTurn = canUseBonusesThisTurn;
        }
    }
}