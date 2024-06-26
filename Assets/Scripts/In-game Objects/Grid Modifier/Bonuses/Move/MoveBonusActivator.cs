using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class MoveBonusActivator : BonusActivator
    {
        public override BonusType BonusType => BonusType.Move;
        public override bool ShouldBlockGrid => true;
        public override bool ShouldBlockHand => false;

        private const int RowsAmount = GameConstants.RowsAmount;
        private const int ColumnsAmount = GameConstants.ColumnsAmount;

        [Header("Bonus Specific Elements")]
        [SerializeField] private Button upButton;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private Button downButton;

        private Func<bool> isAnyCellOnGrid;

        [Inject]
        private void Construct(GameplayGrid gameplayGrid)
        {
            this.isAnyCellOnGrid = () => gameplayGrid.HasAnyColoredCell;
        }

        protected override void InitializeObject()
        {
            upButton.onClick.AddListener(ShiftRowsUp);
            leftButton.onClick.AddListener(ShiftColumnsLeft);
            rightButton.onClick.AddListener(ShiftColumnsRight);
            downButton.onClick.AddListener(ShiftRowsDown);
        }

        public override void Activate()
        {
            base.Activate();

            if(isAnyCellOnGrid.Invoke() == false)
                Deactivate();
        }

        private void ShiftRowsUp()
        {
            List<CellColor> firstRowColors = cellsMatrix.GetRowElements(0).Select(x => x.CellColor).ToList();

            for (int i = 0; i < RowsAmount - 1; i++)
            {
                for (int j = 0; j < ColumnsAmount; j++)
                {
                    cellsMatrix[i, j].UpdateColor(cellsMatrix[i + 1, j].CellColor);
                }
            }

            for (int i = 0; i < ColumnsAmount; i++)
            {
                cellsMatrix[RowsAmount - 1, i].UpdateColor(firstRowColors[i]);
            }

            Deactivate();
        }

        private void ShiftRowsDown()
        {
            List<CellColor> lastRowColors = cellsMatrix.GetRowElements(RowsAmount - 1).Select(x => x.CellColor).ToList();

            for (int i = RowsAmount - 1; i > 0; i--)
            {
                for (int j = 0; j < ColumnsAmount; j++)
                {
                    cellsMatrix[i, j].UpdateColor(cellsMatrix[i - 1, j].CellColor);
                }
            }

            for (int i = 0; i < ColumnsAmount; i++)
            {
                cellsMatrix[0, i].UpdateColor(lastRowColors[i]);
            }

            Deactivate();
        }

        private void ShiftColumnsLeft()
        {
            List<CellColor> firstColumnColors = cellsMatrix.GetColumnElements(0).Select(x => x.CellColor).ToList();

            for (int j = 0; j < ColumnsAmount - 1; j++)
            {
                for (int i = 0; i < RowsAmount; i++)
                {
                    cellsMatrix[i, j].UpdateColor(cellsMatrix[i, j + 1].CellColor);
                }
            }

            for (int i = 0; i < RowsAmount; i++)
            {
                cellsMatrix[i, ColumnsAmount - 1].UpdateColor(firstColumnColors[i]);
            }

            Deactivate();
        }

        private void ShiftColumnsRight()
        {
            List<CellColor> lastColumnColors = cellsMatrix.GetColumnElements(ColumnsAmount - 1).Select(x => x.CellColor).ToList();

            for (int j = ColumnsAmount - 1; j > 0; j--)
            {
                for (int i = 0; i < RowsAmount; i++)
                {
                    cellsMatrix[i, j].UpdateColor(cellsMatrix[i, j - 1].CellColor);
                }
            }

            for (int i = 0; i < RowsAmount; i++)
            {
                cellsMatrix[i, 0].UpdateColor(lastColumnColors[i]);
            }

            Deactivate();
        }

        private void OnDestroy()
        {
            upButton.onClick.RemoveListener(ShiftRowsUp);
            leftButton.onClick.RemoveListener(ShiftColumnsLeft);
            rightButton.onClick.RemoveListener(ShiftColumnsRight);
            downButton.onClick.RemoveListener(ShiftRowsDown);
        }
    }
}