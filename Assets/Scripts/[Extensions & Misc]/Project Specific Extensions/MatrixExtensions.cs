using System;
using System.Collections.Generic;

namespace CGames
{
    public static class MatrixExtensions
    {
        /// <summary> An exception that will be thrown when the given matrix is empty. </summary>
        private readonly static ArgumentException MainMatrixException = new ("Given matrix is empty.");

        /// <summary> Invokes a given action for each of the matrix elements. </summary>
        public static void ForEach<T>(this T[,] self, Action<T> action)
        {
            if(self == null || self.Length == 0)
                throw MainMatrixException;
            
            for (int row = 0; row < GameConstants.RowsAmount; row++)
            {
                for (int column = 0; column < GameConstants.ColumnsAmount; column++)
                {
                    action?.Invoke(self[row, column]);
                }
            }
        }

        /// <summary> Invokes a given action for each of the matrix elements. </summary>
        /// <remarks> Used for actions that needs to know element's indexes </remarks>
        /// <param name="action"> Action that takes: T - element, int - row index, int - column index. </param>
        public static void ForEachIndexed<T>(this T[,] self, Action<T, int, int> action)
        {
            if(self == null || self.Length == 0)
                throw MainMatrixException;

            for (int i = 0; i < GameConstants.RowsAmount; i++)
            {
                for (int j = 0; j < GameConstants.ColumnsAmount; j++)
                {
                    action?.Invoke(self[i, j], i, j);
                }
            }
        }

        /// <returns> List of elements from target row. </returns>
        public static List<T> GetRowElements<T>(this T[,] self, int rowIndex)
        {
            if(self == null || self.Length == 0)
                throw MainMatrixException;

            if(rowIndex >= GameConstants.RowsAmount)
                throw new ArgumentOutOfRangeException($"Index ({rowIndex} is out of range. Max possible value is {GameConstants.RowsAmount - 1})");

            List<T> rowElements = new(self.GetLength(1));

            for (int column = 0; column < self.GetLength(1); column++)
            {
                rowElements.Add(self[rowIndex, column]);
            }

            return rowElements;
        }

        /// <returns> List of elements from target column. </returns>
        public static List<T> GetColumnElements<T>(this T[,] self, int columnIndex)
        {
            if(self == null || self.Length == 0)
                throw MainMatrixException;

            if(columnIndex >= GameConstants.ColumnsAmount)
                throw new ArgumentOutOfRangeException($"Index ({columnIndex} is out of range. Max possible value is {GameConstants.ColumnsAmount - 1})");

            List<T> columnElements = new(self.GetLength(0));

            for (int row = 0; row < self.GetLength(0); row++)
            {
                columnElements.Add(self[row, columnIndex]);
            }

            return columnElements;
        }

        /// <returns> Returns true if any of the elements from matrix match prediction. </returns>
        public static bool Any<T>(this T[,] self, Predicate<T> predicate)
        {
            if(self == null || self.Length == 0)
                throw MainMatrixException;

            foreach (T element in self)
            {
                if (predicate(element))
                    return true;
            }

            return false;
        }

        /// <returns> Returns true if all elements from matrix match prediction. </returns>
        public static bool All<T>(this T[,] self, Predicate<T> predicate)
        {
            if(self == null || self.Length == 0)
                throw MainMatrixException;

            foreach(T element in self)
            {
                if(predicate(element) == false)
                    return false;
            }

            return true;
        }
    }
}