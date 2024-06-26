using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace CGames
{
    public static class ListExtensions
    {
        /// <summary> An exception that will be thrown when the given list is empty. </summary>
        public readonly static ArgumentException MainListException = new ("Given list is empty.");
            
        /// <returns> Returns whether given list has zero items in it. </returns>
        public static bool IsEmpty<T>(this IList<T> list) => list.Count == 0;

        /// <returns> Returns whether given list is null or has zero items in it. </returns>
        public static bool IsNullOrEmpty<T>(this IList<T> list) => list == null || list.Count == 0;

        /// <returns> Returns a random value from a given List. </returns>
        public static T GetRandomItem<T>(this IList<T> list) => list[Random.Range(0, list.Count)];

        /// <summary> Returns a random value from a given list. That value will have a Hash Code that will differ from a given Hash Code. </summary>
        public static T GetRandomUniqueItem<T>(this IList<T> list, T itemToIgnore)
        {
            if (list.IsEmpty()) 
                throw MainListException;

            T randomValue;

            do
                randomValue = list.GetRandomItem();
            while 
                (randomValue.GetHashCode() == itemToIgnore.GetHashCode());

            return randomValue;
        }

        /// <summary> Returns a random value from a given list. That value will have a Hash Code that will differ from a given List of Hash Codes. </summary>
        public static T GetRandomUniqueItem<T>(this IList<T> list, List<T> itemsToIgnore)
        {
            if (list.IsEmpty()) 
                throw MainListException;

            List<int> hashCodeToCheckList = itemsToIgnore.Select(x => x.GetHashCode()).ToList();
            T randomValue;

            do
                randomValue = list.GetRandomItem();
            while 
                (hashCodeToCheckList.Contains(randomValue.GetHashCode()));

            return randomValue;
        }

        /// <summary> Randomly shuffles all items in a given list. </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            if (list.IsEmpty()) 
                throw MainListException;
            
            for (int i = list.Count - 1; i > 1; i--)
            {
                int j = Random.Range(0, i + 1);
                
                T value = list[j];
                list[j] = list[i];
                list[i] = value;
            }
        }

        /// <returns> Returns true, if all values in the list are ascending </returns>
        public static bool AreValuesAscending(this List<int> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] <= list[i - 1])
                    return false;
            }

            return true;
        }

        /// <returns> Returns true, if all values in the list are descending </returns>
        public static bool AreValuesDescending(this List<int> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] > list[i - 1])
                    return false;
            }
            
            return true;
        }
    }
}