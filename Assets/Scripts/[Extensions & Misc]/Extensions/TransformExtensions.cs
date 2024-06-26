using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CGames
{
    public static class TransformExtensions
    {
        /// <summary> Destroys all Child GameObjects in given parent Transform. </summary>
        public static void DestroyAllChildObjects(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
        }

        /// <returns> True if this transform has child gameObjects </returns>
        public static bool HasChildren(this Transform transform) => transform.childCount > 0;

        /// <summary> Gets position on all transform elements from the given list. </summary>
        /// <returns> New List with Vector3 which shows objects' positions. </returns>
        public static List<Vector3> GetPositions(this IList<Transform> list)
        {
            if (list.IsEmpty()) 
                throw ListExtensions.MainListException;
            else
                return list.Select(x => x.position).ToList();
        }
    }
}