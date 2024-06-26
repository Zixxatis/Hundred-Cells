using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace CGames
{
    /// <summary> Enum Extension Methods </summary>
    /// <typeparam name="T"> type of Enum </typeparam>
    public static class Enum<T> where T : Enum
    {
        /// <summary> Returns a number of elements in the selected Enum. </summary>  
        public static int Length => Enum.GetNames(typeof(T)).Length;

        /// <summary> Returns a random value from a given enum. </summary>  
        public static T GetRandomValue()
        {   
            Array values = Enum.GetValues(typeof(T));
            int randomIndex = Random.Range(0, Length);
            return (T)values.GetValue(randomIndex);
        }

        /// <summary> Returns a random value from a given enum, excluding one with given index. </summary>  
        /// <remarks> Works correctly ONLY with Enums with unchanged indexation. </remarks>
        public static T GetRandomValueWithException(int exceptionIndex)
        {
            Array values = Enum.GetValues(typeof(T));
            int randomIndex;

            do
                randomIndex = Random.Range(0, Length);
            while
                (randomIndex == exceptionIndex);

            return (T)values.GetValue(randomIndex);
        }

        /// <summary> Returns a random value from a given enum, excluding values from given list of indexes. </summary>  
        /// <remarks> Works correctly ONLY with Enums with unchanged indexation. </remarks>
        public static T GetRandomValueWithException(List<int> exceptionIndexesList)
        {
            Array values = Enum.GetValues(typeof(T));
            int randomIndex;

            do
                randomIndex = Random.Range(0, Length);
            while
                (exceptionIndexesList.Contains(randomIndex));

            return (T)values.GetValue(randomIndex);
        }
    }
}