using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CGames
{
    public static class ShapeCoordinatesAssigner
    {
        /// <returns> Returns a Dictionary with rotation degrees and coordinates of all cells in List<Vector2Int> to that rotation. </returns>
        public static Dictionary<RotationDegrees, List<Vector2Int>> CreateCoordinatesDictionary(List<Vector2Int> defaultLocalCellsCoordinates)
        {
            Dictionary<RotationDegrees, List<Vector2Int>> cellsCoordinationsDictionary = new()
            {
                { RotationDegrees.None, defaultLocalCellsCoordinates }
            };

            while(cellsCoordinationsDictionary.Count != Enum<RotationDegrees>.Length)
            {
                KeyValuePair<RotationDegrees, List<Vector2Int>> previousEntry = cellsCoordinationsDictionary.Last();

                cellsCoordinationsDictionary.Add
                (
                    RotationDegreesUtilities.GetNextRotationDegrees(previousEntry.Key), 
                    GetNextCoordinates(previousEntry.Value)
                );
            }

            return cellsCoordinationsDictionary;
        }
 
        private static List<Vector2Int> GetNextCoordinates(List<Vector2Int> previousCoordinatesList)
        {
            List<Vector2Int> newCoordinatesList = new();

            foreach (Vector2Int oldCoordinates in previousCoordinatesList)
            {
                Vector2Int newCoordinates = new(GameConstants.MaxShapeDimensionsInCells - 1 - oldCoordinates.y, oldCoordinates.x);
                newCoordinatesList.Add(newCoordinates);
            }

            return MoveTowardsZero(newCoordinatesList.OrderBy(coordinates => coordinates.x).ThenBy(coordinates => coordinates.y).ToList());            
        }

        /// <summary> Will move all coordinates towards "0", until any of the cells will appear on "0" on X and Y. </summary>
        private static List<Vector2Int> MoveTowardsZero(List<Vector2Int> newCoordinatesList)
        {
            while (newCoordinatesList.Exists(coordinates => coordinates.x == 0) == false)
            {
                newCoordinatesList = newCoordinatesList.Select(coordinates => new Vector2Int(coordinates.x - 1, coordinates.y)).ToList();
            }

            while (newCoordinatesList.Exists(coordinates => coordinates.y == 0) == false)
            {
                newCoordinatesList = newCoordinatesList.Select(coordinates => new Vector2Int(coordinates.x, coordinates.y - 1)).ToList();
            }

            return newCoordinatesList;
        }
    }
}