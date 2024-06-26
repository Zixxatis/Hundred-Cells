using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    public static class GameObjectExtensions
    {
        /// <summary> Activates object in hierarchy. Similar to Unity Engine's 'SetActive(true)' </summary>
        public static void ActivateObject(this GameObject gameObject) => gameObject.SetActive(true);

        /// <summary> Deactivates object in hierarchy. Similar to Unity Engine's 'SetActive(false)' </summary>
        public static void DeactivateObject(this GameObject gameObject) => gameObject.SetActive(false);

        /// <summary> Activates all gameObjects from a given list. </summary>
        public static void ActivateObjects(this IList<GameObject> list)
        {
            if (list.IsEmpty()) 
                throw ListExtensions.MainListException;

            foreach (GameObject gameObject in list)
            {
                gameObject.ActivateObject();
            }
        }

        /// <summary> Deactivates all gameObjects from a given list. </summary>
        public static void DeactivateObjects(this IList<GameObject> list)
        {
            if (list.IsEmpty()) 
                throw ListExtensions.MainListException;

            foreach (GameObject gameObject in list)
            {
                gameObject.DeactivateObject();
            }
        }
    }
}