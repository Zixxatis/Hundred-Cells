using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class GameplayGrid : MonoBehaviour, ISavable<SessionData>, IDimensionsNotifier
    {
        private IGameOverNotifier gameOverNotifier;
        private GridLayoutGroup gridLayoutGroup;
        private readonly Cell[,] cellsMatrix = new Cell[GameConstants.RowsAmount, GameConstants.ColumnsAmount];
        
        public bool HasAnyColoredCell => cellsMatrix.Any(x => x.IsFilled);
        Vector2 IDimensionsNotifier.CellDimensions => new(gridLayoutGroup.cellSize.x, gridLayoutGroup.cellSize.y);
        Vector2 IDimensionsNotifier.GridSpacing => new(gridLayoutGroup.spacing.x, gridLayoutGroup.spacing.y);

        [Inject]
        private void Construct(GridModifier gridModifier, GridMediator gridMediator, IGameOverNotifier gameOverNotifier)
        {
            this.gameOverNotifier = gameOverNotifier;

            PrepareGridMatrix();
            gridModifier.Initialize(cellsMatrix);
            gridMediator.Initialize(cellsMatrix);
        }

        private void PrepareGridMatrix()
        {
            gridLayoutGroup = GetComponent<GridLayoutGroup>();
            int cellsAmount = gridLayoutGroup.transform.childCount;

            if(cellsAmount != GameConstants.CellsAmount)
                throw new InvalidOperationException($"Grid size doesn't match the expected value ({GameConstants.CellsAmount}). Current size is {cellsAmount}");

            if(cellsAmount != GameConstants.RowsAmount * GameConstants.ColumnsAmount)
                throw new InvalidOperationException("The size of the cells array doesn't match the expected matrix dimensions.");
           
            cellsMatrix.ForEachIndexed
            (
                (x, row, column) => cellsMatrix[row, column] = gridLayoutGroup.transform.GetChild(row * GameConstants.ColumnsAmount + column).GetComponent<Cell>() 
            );
        }

        public void ReceiveData(SessionData data)
        {
            if (data.CellsColorData.IsNullOrEmpty() == false)
            {
                List<CellColor> CellsColorData = data.CellsColorData;
                cellsMatrix.ForEachIndexed((x, row, column) => x.UpdateColor(CellsColorData[row * GameConstants.RowsAmount + column]));
            }
            else
                cellsMatrix.ForEach((x) => x.UpdateColor(CellColor.None));
        }

        public void PassData(SessionData data)
        {
            List<CellColor> cellsColorData = new();
            cellsMatrix.ForEachIndexed((x, row, column) => cellsColorData.Add(cellsMatrix[row, column].CellColor));

            data.CellsColorData = cellsColorData;
        }

        private void Awake()
        {
            gameOverNotifier.OnGameOver += ClearGrid;
        }

        [ContextMenu("Clear Grid")]
        private void ClearGrid() => cellsMatrix.ForEach(x => x.ClearCell());

        private void OnDestroy()
        {
            gameOverNotifier.OnGameOver -= ClearGrid;
        }
    }
}