using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CGames.CustomEditors
{
    public class ReferencesListWindow : ReferencesWindow
    {
        private List<ILocalizable> referencesList = new();

        public void PassReferences(List<ILocalizable> referencesList, string key, ObjectsSearchType objectsSearchType)
        {
            this.referencesList = referencesList;
            this.key = key;
            this.objectsSearchType = objectsSearchType;

            isReadyToDisplay = true;
        }

        protected override void DrawEntries()
        {
            GUILayout.Label($"References for \"{key}\" key in {objectsSearchType}:", EditorStyles.largeLabel);
            GUILayout.Space(10);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            DrawLocalizableObjectsList(referencesList);

            EditorGUILayout.EndScrollView();
        }
    }
}