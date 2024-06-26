using UnityEngine;
using UnityEditor;

namespace CGames.CustomEditors
{
    public static class GameObjectCreator
    {
        public static void CreateObject(string prefabFileName)
        {
            GameObject selectedParent = Selection.activeGameObject;

            if (selectedParent != null)
            { 
                GameObject prefab = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>($"[Prefabs for Instantiating]/{prefabFileName}")) as GameObject;

                prefab.transform.SetParent(selectedParent.transform);

                prefab.transform.localPosition = Vector3.zero;
                prefab.transform.localScale = new(1, 1, 1);

                PrefabUtility.UnpackPrefabInstance(prefab, PrefabUnpackMode.Completely, InteractionMode.UserAction);

                Selection.activeGameObject = prefab;
            }
            else
                Debug.LogWarning("Please select a parent object in the scene.");
        }
    }
}