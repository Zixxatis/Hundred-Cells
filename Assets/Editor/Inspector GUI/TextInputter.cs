using UnityEngine;
using UnityEditor;

namespace CGames.CustomEditors
{
    [CustomEditor(typeof(TextLocalizer)), CanEditMultipleObjects]
    public class TextInputter : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(10);

            TextLocalizer textLocalizer = (TextLocalizer)target;

            EditorGUILayout.BeginHorizontal();

            if(GUILayout.Button("Set [ENG] Text"))
            {
                PreviewTextAssistant.SetPreviewLanguage(Language.English);

                textLocalizer.UpdateText();

                EditorUtility.SetDirty(textLocalizer);
                AssetDatabase.SaveAssets();
            }

            if(GUILayout.Button("Set [RU] Text"))
            {
                PreviewTextAssistant.SetPreviewLanguage(Language.Russian);

                textLocalizer.UpdateText();

                EditorUtility.SetDirty(textLocalizer);
                AssetDatabase.SaveAssets();
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            if(GUILayout.Button("Reload Dictionary"))
            {
                LocalizationDictionary.UpdateDictionary();
                Debug.Log("Dictionary updated successfully.");
            }
        }
    }
}