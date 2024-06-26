using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CGames
{
    public class GridValidator
    {
        private readonly Cell[,] cellsMatrix;

        public GridValidator(Cell[,] cellsMatrix)
        {
            this.cellsMatrix = cellsMatrix;
        }
      
        /// <returns> Return true if any line can be removed. </returns>
        /// <remarks> "OUT" is dictionary with KVP of cell from filled lines & multiplier given for it's removal. </remarks>
        public bool TryToFindFilledLines(out Dictionary<Cell, byte> filledCellsWithMultipliersDictionary)
        {
            filledCellsWithMultipliersDictionary = new();
            byte multiplier = 1;

            for (int i = 0; i < GameConstants.RowsAmount; i++)
            {
                List<Cell> cellsInRow = cellsMatrix.GetRowElements(i);

                if(cellsInRow.TrueForAll(x => x.IsFilled))
                {
                    foreach (Cell cell in cellsInRow)
                    {
                        filledCellsWithMultipliersDictionary.Add(cell, multiplier);
                    }

                    multiplier ++;
                }
            }

            for (int i = 0; i < GameConstants.ColumnsAmount; i++)
            {
                List<Cell> cellsInColumns = cellsMatrix.GetColumnElements(i);

                if(cellsInColumns.TrueForAll(x => x.IsFilled))
                {
                    foreach (Cell cell in cellsInColumns)
                    {
                        if(filledCellsWithMultipliersDictionary.ContainsKey(cell))
                            continue;

                        filledCellsWithMultipliersDictionary.Add(cell, multiplier);
                    }

                    multiplier ++;
                }
            }

            return filledCellsWithMultipliersDictionary.Count > 0;
        }

        /// <returns> Returns true, if any shape from hand can be placed on the grid. </returns>
        public bool CanPlaceAnyShapeFromHand(IEnumerable<Shape> shapesInHand) => shapesInHand.Any(CanPlaceSpecificShape);

        private bool CanPlaceSpecificShape(Shape shape)
        {
            List<Vector2Int> cellsCoordinates = shape.CellsCoordinates;

            for (int i = 0; i < GameConstants.RowsAmount; i++)
            {
                for (int j = 0; j < GameConstants.ColumnsAmount; j++)
                {
                    if(CanPlaceShapeAtPosition(cellsCoordinates, i, j))
                        return true;
                }
            }
            
            return false;
        }

        private bool CanPlaceShapeAtPosition(List<Vector2Int> shapeCellsCoordinates, int startRow, int startColumn)
        {
            foreach (Vector2Int coordinates in shapeCellsCoordinates)
            {
                int targetRow = startRow + coordinates.x;
                int targetColumn = startColumn + coordinates.y;

                if (targetRow < 0 || targetRow >= GameConstants.RowsAmount)
                    return false;
                
                if(targetColumn < 0 || targetColumn >= GameConstants.ColumnsAmount)
                    return false;

                if (cellsMatrix[targetRow, targetColumn].IsFilled)
                    return false;
            }

            return true;
        }
    }
}