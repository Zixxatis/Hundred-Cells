using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace CGames.CustomEditors
{
    [CustomEditor(typeof(EncryptionConfig))]
    public class EncryptionConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(10);

            if (GUILayout.Button("Generate Code Word"))
            {
                CodeGenerationWindow.ShowWindow((EncryptionConfig)target);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }
    }
}