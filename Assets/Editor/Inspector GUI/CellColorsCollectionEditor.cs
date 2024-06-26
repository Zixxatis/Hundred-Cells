using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace CGames.CustomEditors
{
    [CustomEditor(typeof(CellColorsCollection))]
    public class CellColorsCollectionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(10);

            if (GUILayout.Button("Create / Reset List To Default"))
            {
                MethodInfo method = target.GetType().GetMethod("CreateNewList", BindingFlags.NonPublic | BindingFlags.Instance);

                if (method == null)
                {
                    Debug.LogWarning($"Method \"CreateNewList\" not found in {target.GetType().Name}");
                    return;
                }
        
                method.Invoke(target, null);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }
    }
}