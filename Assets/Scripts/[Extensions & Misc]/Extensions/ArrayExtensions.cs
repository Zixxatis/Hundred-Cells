using System;

namespace CGames
{
    public static class ArrayExtensions
    {
        /// <summary> An exception that will be thrown when the given array is empty. </summary>
        private readonly static ArgumentException MainArrayException = new ("Given array is empty.");

        /// <returns> First object of collection. </returns>
        public static T FirstElement<T>(this T[] self) => self[0];

        /// <returns> Last object of collection. </returns>
        public static T LastElement<T>(this T[] self) => self[^1];

        /// <summary> Check collection length. </summary>
        /// <returns> True if collection is empty </returns>
        public static bool IsEmpty<T>(this T[] self) => (self.Length < 1);

        /// <returns> Random value of collection. </returns>
        public static T GetRandomElement<T>(this T[] self)
        {
            if (self.IsEmpty()) 
                throw MainArrayException;
            else
                return self[UnityEngine.Random.Range(0, self.Length - 1)];
        }
    }
}