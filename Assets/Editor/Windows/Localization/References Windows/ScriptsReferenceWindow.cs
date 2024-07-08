using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CGames.CustomEditors
{
    public class ScriptsReferenceWindow : ReferencesWindow
    {
        private KeyValuePair<string, List<string>> referencesInScriptsKVP = new();
        
        public void PassReferences(KeyValuePair<string, List<string>> referencesInScriptsKVP, ObjectsSearchType objectsSearchType)
        {
            this.referencesInScriptsKVP = referencesInScriptsKVP;
            this.key = referencesInScriptsKVP.Key;
            this.objectsSearchType = objectsSearchType;

            isReadyToDisplay = true;
        }

        protected override void DrawEntries()
        {
            GUILayout.Label($"References for \"{key}\" key in {objectsSearchType}:", EditorStyles.largeLabel);
            GUILayout.Space(10);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            foreach (string pathToScript in referencesInScriptsKVP.Value)
            {
                // ? Trims path to 'Assets/...'
                string pathInAssets = pathToScript[pathToScript.IndexOf("Assets")..];

                Object scriptObject = AssetDatabase.LoadAssetAtPath(pathInAssets, typeof(Object));
                EditorGUILayout.ObjectField(scriptObject, typeof(Object), true);
            }

            EditorGUILayout.EndScrollView();
        }
    }
}