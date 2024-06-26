using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CGames.CustomEditors
{
    public abstract class ReferencesWindow : EditorWindow
    {
        protected string key;
        protected ObjectsSearchType objectsSearchType;

        protected bool isReadyToDisplay;
        protected Vector2 scrollPosition = Vector2.zero;

        public static void ShowWindow() => GetWindow<ReferencesListWindow>();

        private void OnGUI()
        {
            if(isReadyToDisplay == false)
                return;

            DrawEntries();
        }

        protected abstract void DrawEntries();

        protected static void DrawLocalizableObjectsList(List<ILocalizable> referencesList)
        {
            foreach (ILocalizable localizableObject in referencesList)
            {
                EditorGUILayout.ObjectField((Object)localizableObject, typeof(Object), true);
            }
        }
    }

    public enum ObjectsSearchType
    {
        GameObjects,
        Prefabs,
        ScriptableObjects,
        Scripts
    }
}