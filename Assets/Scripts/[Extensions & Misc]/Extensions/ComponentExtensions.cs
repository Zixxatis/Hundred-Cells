using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    public static class ComponentExtensions
    {
        /// <summary> Changes object's 'Active' status in hierarchy. Similar to Unity Engine's 'component.gameObject.SetActive(bool) </summary>
        public static void ChangeGameObjectActivation(this Component component, bool shouldActivate) => component.gameObject.SetActive(shouldActivate);

        /// <summary> Activates object in hierarchy. Similar to Unity Engine's "Component.gameObject.SetActive(true)" </summary>
        public static void ActivateGameObject(this Component component) => component.gameObject.SetActive(true);

        /// <summary> Deactivates object in hierarchy. Similar to Unity Engine's 'component.gameObject.SetActive(false)' </summary>
        public static void DeactivateGameObject(this Component component) => component.gameObject.SetActive(false);

        /// <summary> Activates all gameObjects from a given list. </summary>
        public static void ActivateObjects<T>(this IList<T> list) where T : Component
        {
            if (list.IsEmpty()) 
                throw ListExtensions.MainListException;

            foreach (Component comp in list) 
            {
                comp.gameObject.ActivateObject();
            }
        }

        /// <summary> Deactivates all gameObjects from a given list. </summary>
        public static void DeactivateObjects<T>(this IList<T> list) where T : Component
        {
            if (list.IsEmpty()) 
                throw ListExtensions.MainListException;

            foreach (Component comp in list) 
            {
                comp.gameObject.DeactivateObject();
            }
        }
    }
}