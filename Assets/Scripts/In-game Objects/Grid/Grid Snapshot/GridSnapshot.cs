using System;
using System.Collections.Generic;
using System.Linq;

namespace CGames
{
    public class GridSnapshot : ISnapshotSaver, ISnapshotLoader
    {
        private readonly Func<GameMode> getCurrentGameMode;
        private readonly Func<IEnumerable<Shape>> getShapesInPlayerHand;
        private readonly Func<uint> getCurrentScore;
        private readonly Func<bool> getBonusStatusForThisTurnStatus;

        private GameMode storedGameMode;
        private List<ShapeData> storedShapeDataInHand;
        private uint storedScore;
        private readonly List<CellColor> storedCellColors;
        private Cell[,] cellsMatrix;
        private bool canUseBonusesThisTurn;

        private bool hasCapturedData;

        public GridSnapshot(GameStatusHandler gameStatusHandler, PlayerHand playerHand, ScoreCounter scoreCounter, BonusSystem bonusSystem) 
        {
            this.getCurrentGameMode = () => gameStatusHandler.GameMode;
            this.getShapesInPlayerHand = () => playerHand.ShapesInHand;
            this.getCurrentScore = () => scoreCounter.CurrentScore;
            this.getBonusStatusForThisTurnStatus = () => bonusSystem.CanUseBonusesThisTurn;

            storedCellColors = new(GameConstants.CellsAmount);
        }

        public void Initialize(Cell[,] cellsMatrix)
        {
            this.cellsMatrix = cellsMatrix;
        }

        public void CaptureData()
        {
            storedGameMode = getCurrentGameMode();
            storedShapeDataInHand = getShapesInPlayerHand().Select(x => (x == null)? null : x.ShapeData.Clone()).ToList();
            storedScore = getCurrentScore();

            if(storedCellColors.Count == 0)
                cellsMatrix.ForEachIndexed((x, row, column) => storedCellColors.Add(cellsMatrix[row, column].CellColor));
            else
                cellsMatrix.ForEachIndexed((x, row, column) => storedCellColors[row * 10 + column] = cellsMatrix[row, column].CellColor);

            canUseBonusesThisTurn = getBonusStatusForThisTurnStatus.Invoke();

            hasCapturedData = true;
        }

        public SessionData GetDataFromSnapshot()
        {
            return new SessionData
            (
                storedGameMode,
                storedScore,
                storedShapeDataInHand,
                storedCellColors,
                canUseBonusesThisTurn
            );
        }

        public bool HasCapturedData() => hasCapturedData;
    }
}