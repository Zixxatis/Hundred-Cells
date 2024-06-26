using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CGames.CustomEditors
{
    public class ScenesReferenceWindow : ReferencesWindow
    {
        private List<KeyValuePair<string, ILocalizable>> referencesInScenesList = new();
        
        public void PassReferences(List<KeyValuePair<string, ILocalizable>> referencesInScenesList, string key, ObjectsSearchType objectsSearchType)
        {
            this.referencesInScenesList = referencesInScenesList;
            this.key = key;
            this.objectsSearchType = objectsSearchType;

            isReadyToDisplay = true;
        }

        protected override void DrawEntries()
        {
            GUILayout.Label($"References for \"{key}\" key in {objectsSearchType}:", EditorStyles.largeLabel);
            GUILayout.Label($"Please, note that only references for current scene will be shown.");
            GUILayout.Space(10);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            Dictionary<string, List<ILocalizable>> sortedByScenesDictionary = referencesInScenesList.GroupBy(x => x.Key).ToDictionary
            (
                y => y.Key, z => z.Select(pair => pair.Value).ToList()
            );

            foreach (KeyValuePair<string, List<ILocalizable>> kvp in sortedByScenesDictionary)
            {
                GUILayout.Label($"In \"{kvp.Key}\" scene:");

                DrawLocalizableObjectsList(kvp.Value);
            }

            EditorGUILayout.EndScrollView();
        }
    }
}