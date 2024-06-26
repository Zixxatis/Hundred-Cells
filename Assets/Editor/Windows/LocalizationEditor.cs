using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace CGames.CustomEditors
{
    public class LocalizationEditor : EditorWindow
    {
        private const int DEFAULT_HEIGHT = 20;
        private const int MAX_CHARS_IN_LINE = 40;

        private readonly string separator = $"\",\"";
        private string csvFilePath = "Assets/Resources/Localization.csv";
        private RowData headersRow;
        private List<RowData> dataRows;
        private bool IsDataLoaded => dataRows != null && dataRows.Count > 0;

        private string newLanguageIndex = string.Empty;
        private string newEntryKey = string.Empty;
        private string searchQuery = string.Empty;
        
        private List<KeyValuePair<string, ILocalizable>> gameObjectsInScenesReferenceList;
        private List<ILocalizable> referencesInPrefabsList;
        private List<ILocalizable> referencesInScriptableObjectsList;
        private Dictionary<string, List<string>> referencesInScriptsDictionary;

        private Vector2 scrollPosition = Vector2.zero;
        

        [MenuItem("Tools/Editor Windows/Localization Editor")]
        public static void Init()
        {
            LocalizationEditor localizationEditor = GetWindow<LocalizationEditor>("Localization Editor");

            localizationEditor.saveChangesMessage = "You've made changes to the dictionary. Would you like to save them?";
            localizationEditor.Show();
        }

        private void OnGUI()
        {
            if (IsDataLoaded == false)
            {
                DrawLoadingFields();
                return;
            }

            // ? Listener for "Ctrl + S" shortcut to save changes.
            if (Event.current.type == EventType.KeyDown && Event.current.control && Event.current.keyCode == KeyCode.S)
            {
                SaveChanges();
                Event.current.Use();
            }

            DrawActionButtons();
                GUILayout.Space(10);
            DrawCreationFields();
                GUILayout.Space(10);
            DrawSearchField();
                GUILayout.Space(10);
            DrawDataBaseButton();
                GUILayout.Space(10);
            DrawHeaders();
            DrawDataFields();
        }

    #region Drawing Elements
        private void DrawLoadingFields()
        {
            csvFilePath = EditorGUILayout.TextField("Path to file:", csvFilePath);

            if (GUILayout.Button("Load CSV"))
                LoadCSV();
        }

        private void DrawActionButtons()
        { 
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Save Changes"))
                SaveChanges();

            GUILayout.Space(5);

            if (GUILayout.Button("Discard Changes"))
                DiscardChanges();

            GUILayout.Space(5);

            if (GUILayout.Button("Sort by Tags"))
                SortEntries();

            EditorGUILayout.EndHorizontal();
        }

        private void DrawCreationFields()
        {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("Language to add: ", GUILayout.Width(110));
            newLanguageIndex = EditorGUILayout.TextField(newLanguageIndex);
            
            GUILayout.Space(5);

            if (GUILayout.Button("Add new language", GUILayout.Width(350)))
                AddNewLanguage();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Key to add: ", GUILayout.Width(110));
            newEntryKey = EditorGUILayout.TextField(newEntryKey);

            GUILayout.Space(5);

            if (GUILayout.Button("Add new entry", GUILayout.Width(350)))
                AddNewEntry();

            EditorGUILayout.EndHorizontal();
        }

        private void DrawSearchField()
        {
            GUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("Search: ", GUILayout.Width(50), GUILayout.Height(DEFAULT_HEIGHT));
            searchQuery = GUILayout.TextField(searchQuery, GUILayout.Height(DEFAULT_HEIGHT));

            if(GUILayout.Button("Find", GUILayout.Width(60), GUILayout.Height(DEFAULT_HEIGHT)))
                dataRows.ForEach(x => x.shouldBeDisplayed = x.cells.Exists(y => y.ToLower().Contains(searchQuery.ToLower())));

            if (GUILayout.Button("Clear", GUILayout.Width(45), GUILayout.Height(DEFAULT_HEIGHT)))
            {
                dataRows.TrueForAll(x => x.shouldBeDisplayed = true);
                searchQuery = string.Empty;
            }

            GUILayout.EndHorizontal();
        }

        private void DrawDataBaseButton()
        {
            if(GUILayout.Button("Update reference data base", GUILayout.Height(DEFAULT_HEIGHT)))
                UpdateReferenceDataBase();
        }

        private void DrawHeaders()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("GObj", CenteredStyle, GUILayout.Width(30), GUILayout.Height(DEFAULT_HEIGHT));
            EditorGUILayout.LabelField("Pref", CenteredStyle, GUILayout.Width(30), GUILayout.Height(DEFAULT_HEIGHT));
            EditorGUILayout.LabelField("SObj", CenteredStyle, GUILayout.Width(30), GUILayout.Height(DEFAULT_HEIGHT));
            EditorGUILayout.LabelField(".CS", CenteredStyle, GUILayout.Width(30), GUILayout.Height(DEFAULT_HEIGHT));

            for (int i = 0; i < headersRow.cells.Count; i++)
            {
                EditorGUILayout.LabelField(headersRow.cells[i], GUILayout.Height(DEFAULT_HEIGHT));
            }
            
            EditorGUILayout.LabelField("Action", GUILayout.Width(55), GUILayout.Height(DEFAULT_HEIGHT));

            // ? This is an extra create above the scroll view
            GUILayout.Space(10);

            EditorGUILayout.EndHorizontal();            
        }

        private void DrawDataFields()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < dataRows.Count; i++)
            {
                if(dataRows[i].shouldBeDisplayed == false)
                    continue;
                
                EditorGUILayout.BeginHorizontal();

                if(dataRows[i].HasReferencesInGameObjects())
                {
                    if(GUILayout.Button
                    (
                        new GUIContent($"x{dataRows[i].GetGameObjectsReferenceAmount()}", $"This entry has x{dataRows[i].GetGameObjectsReferenceAmount()} reference(s) in GameObjects. Press this button to view them."), 
                        GUILayout.Width(30), 
                        GUILayout.Height(DEFAULT_HEIGHT)
                    ))
                    {
                        ScenesReferenceWindow window = GetWindow<ScenesReferenceWindow>();
                        window.PassReferences
                        (
                            gameObjectsInScenesReferenceList.Where(x => x.Value.HasLocalizationKey(dataRows[i].Key)).ToList(),
                            dataRows[i].Key,
                            ObjectsSearchType.GameObjects                            
                        );
                    }
                }
                else
                    EditorGUILayout.LabelField(dataRows[i].GetGameObjectsReferenceAmount(), CenteredStyle, GUILayout.Width(30), GUILayout.Height(DEFAULT_HEIGHT));

                if(dataRows[i].HasReferencesInPrefabs())
                {
                    if(GUILayout.Button
                    (
                        new GUIContent($"x{dataRows[i].GetPrefabsReferenceAmount()}", $"This entry has x{dataRows[i].GetPrefabsReferenceAmount()} reference(s) in Prefabs. Press this button to view them."), 
                        GUILayout.Width(30), 
                        GUILayout.Height(DEFAULT_HEIGHT)
                    ))
                    {
                        ReferencesListWindow window = GetWindow<ReferencesListWindow>();
                        window.PassReferences
                        (
                            referencesInPrefabsList.Where(x => x.HasLocalizationKey(dataRows[i].Key)).ToList(),
                            dataRows[i].Key,
                            ObjectsSearchType.Prefabs
                        );
                    }
                }
                else
                    EditorGUILayout.LabelField(dataRows[i].GetPrefabsReferenceAmount(), CenteredStyle, GUILayout.Width(30), GUILayout.Height(DEFAULT_HEIGHT));

                if(dataRows[i].HasReferencesInScriptableObjects())
                {
                    if(GUILayout.Button
                    (
                        new GUIContent($"x{dataRows[i].GetScriptableObjectsReferenceAmount()}", $"This entry has x{dataRows[i].GetScriptableObjectsReferenceAmount()} reference(s) in ScriptableObjects. Press this button to view them."),
                        GUILayout.Width(30), 
                        GUILayout.Height(DEFAULT_HEIGHT)
                    ))
                    {
                        ReferencesListWindow window = GetWindow<ReferencesListWindow>();
                        window.PassReferences
                        (
                            referencesInScriptableObjectsList.Where(x => x.HasLocalizationKey(dataRows[i].Key)).ToList(),
                            dataRows[i].Key,
                            ObjectsSearchType.ScriptableObjects
                        );
                    }
                }
                else
                    EditorGUILayout.LabelField(dataRows[i].GetScriptableObjectsReferenceAmount(), CenteredStyle, GUILayout.Width(30), GUILayout.Height(DEFAULT_HEIGHT));

                if(dataRows[i].HasReferencesInScripts())
                {
                    if(GUILayout.Button
                    (
                        new GUIContent($"x{dataRows[i].GetScriptsReferenceAmount()}", $"This entry has x{dataRows[i].GetScriptsReferenceAmount()} reference(s) in C# scripts. Press this button to view them."), 
                        GUILayout.Width(30), 
                        GUILayout.Height(DEFAULT_HEIGHT)
                    ))
                    {
                        ScriptsReferenceWindow window = GetWindow<ScriptsReferenceWindow>();
                        window.PassReferences
                        (
                            referencesInScriptsDictionary.First(x => x.Key == dataRows[i].Key),
                            ObjectsSearchType.Scripts
                        );
                    }
                }
                else
                    EditorGUILayout.LabelField(dataRows[i].GetScriptsReferenceAmount(), CenteredStyle, GUILayout.Width(30), GUILayout.Height(DEFAULT_HEIGHT));

                for (int j = 0; j < dataRows[i].cells.Count; j++)
                {
                    EditorGUI.BeginChangeCheck();

                    dataRows[i].cells[j] = EditorGUILayout.TextField(dataRows[i].cells[j], TextFieldStyle, GUILayout.MinHeight(DEFAULT_HEIGHT * dataRows[i].HeightMultiplier));

                    if (EditorGUI.EndChangeCheck())
                    {
                        ForceToSaveFile();

                        if(dataRows.Count(x => x.Key == dataRows[i].Key) > 1)
                            Debug.LogWarning($"You already have {dataRows[i].Key} in the dictionary! Only first entry with that key will be used.");
                    }
                }
                
                if(IsDataBaseUpdated() && dataRows[i].HasReferenceInAnyObject() == false)
                {
                    if (GUILayout.Button(new GUIContent("Delete", "This will delete an empty entry."), GUILayout.Width(55), GUILayout.Height(DEFAULT_HEIGHT)))
                    {
                        dataRows.Remove(dataRows[i]);
                        ForceToSaveFile();

                        EditorGUILayout.EndHorizontal();
                        continue;
                    }
                }
                else
                {
                    if(dataRows[i].Key.Equals(dataRows[i].InitialKey) == false && dataRows[i].HasReferenceInAnyObject())
                    {
                        if (GUILayout.Button(new GUIContent("Update", "This set inputted key as initial and will change it in all referenced objects."), GUILayout.Width(55), GUILayout.Height(DEFAULT_HEIGHT)))
                            UpdateFields(dataRows[i]);
                    }
                    else
                        EditorGUILayout.LabelField("", GUILayout.Width(55), GUILayout.Height(DEFAULT_HEIGHT));
                }

                EditorGUILayout.EndHorizontal();

                // ? This will create an extra space between different tags
                if(i != dataRows.Count - 1)
                {
                    if(dataRows[i].KeyTag.Equals(dataRows[i + 1].KeyTag) == false)
                        GUILayout.Space(10);
                }
            }

            EditorGUILayout.EndScrollView();
        }
    #endregion

    #region Functional
        private void LoadCSV()
        {
            if (File.Exists(csvFilePath))
            {
                string[] csvData = File.ReadAllLines(csvFilePath);

                headersRow = new();
                dataRows = new();

                for (int i = 0; i < csvData.Length; i++)
                {
                    string line = csvData[i];

                    if(string.IsNullOrEmpty(line))
                        continue;
                    else
                        line = line[1..^1];

                    string[] keys = line.Split(separator);

                    if(i == 0)
                    {
                        for (int j = 0; j < keys.Length; j++)
                        {
                            headersRow.cells.Add(keys[j]);
                        }

                        headersRow.UpdateInitialKey();
                    }
                    else
                    {
                        RowData rowData = new();

                        for (int j = 0; j < keys.Length; j++)
                        {
                            rowData.cells.Add(keys[j]);
                        }

                        rowData.UpdateInitialKey();
                        dataRows.Add(rowData);
                    }
                }

                newLanguageIndex = string.Empty;
                newEntryKey = string.Empty;
                searchQuery = string.Empty;

                gameObjectsInScenesReferenceList = null;
                referencesInScriptableObjectsList = null;
                referencesInPrefabsList = null;
                referencesInScriptsDictionary = null;

                base.DiscardChanges();

                UpdateReferenceDataBase();
                
                scrollPosition = Vector2.zero;
            }
            else
                Debug.LogError("CSV file not found!");
        }

        public override void SaveChanges()
        {
            if (hasUnsavedChanges == false)
                return;

            List<string> lines = new()
            {
                headersRow.GetRowDataForCSV()
            };

            lines.AddRange(dataRows.Select(x => x.GetRowDataForCSV()));

            if (lines.Count > 0 && string.IsNullOrEmpty(lines[^1]))
                lines = lines.Take(lines.Count - 1).ToList();
            
            File.WriteAllLines(csvFilePath, lines);
            Debug.Log("CSV file saved and updated successfully!");
            
            AssetDatabase.Refresh();
            LoadCSV();

            base.SaveChanges();
        }

        public override void DiscardChanges()
        {
            LoadCSV();

            base.DiscardChanges();
        }

        private void SortEntries()
        {
            dataRows.Sort((a, b) => a.Key.CompareTo(b.Key));

            ForceToSaveFile();
        }

        private void AddNewLanguage()
        {
            if(newLanguageIndex == string.Empty)
            {
                Debug.LogError("Can't add an empty string as a language.");
                return;
            }

            if(headersRow.cells.Contains(newLanguageIndex))
            {
                Debug.LogWarning("You already have this language in the list. Skipped it's creation.");
                newLanguageIndex = string.Empty;
                return;
            }
            
            headersRow.cells.Add(newLanguageIndex);
            
            foreach (RowData row in dataRows)
            {
                row.cells.Add(string.Empty);
            }

            ForceToSaveFile();
            scrollPosition = new(0, 0);
            newLanguageIndex = string.Empty;
        }

        private void AddNewEntry()
        {
            if (newEntryKey == string.Empty)
            {
                Debug.LogError("Can't add an empty key.");
                return;
            }

            List<string> keysList = dataRows.Select(rowList => rowList.Key).ToList();

            if (keysList.Contains(newEntryKey))
            {
                Debug.LogWarning("You already have an entry with that key in the list. Skipped it's creation.");
                newEntryKey = string.Empty;
                return;
            }

            dataRows.Add(new());

            for (int i = 0; i < headersRow.cells.Count; i++)
            {
                if (i == 0)
                {
                    dataRows[^1].cells.Add(newEntryKey);
                    dataRows[^1].UpdateInitialKey();
                }
                else
                    dataRows[^1].cells.Add(string.Empty);
            }

            ForceToSaveFile();
            scrollPosition = new(0, 10000);
            newEntryKey = string.Empty;
        }

        private void UpdateReferenceDataBase()
        {
            FindReferencesInScenes();
            FindReferencesInPrefabs();
            FindReferencesInScriptableObjects();
            FindReferencesInScripts();

            UpdateReferencesCount();

            Debug.Log("Reference data base updated successfully!");
        }

        private void FindReferencesInScenes()
        {
            gameObjectsInScenesReferenceList = new();

            List<string> pathsToScenes = EditorBuildSettings.scenes.Select(x => x.path).ToList();

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                // ? This will additionally open a scene, if it isn't an active scene
                Scene scene = EditorSceneManager.OpenScene(pathsToScenes[i], OpenSceneMode.Additive);

                // ? Gets a root objects from a given scene
                GameObject[] rootGameObjects = scene.GetRootGameObjects();

                // ? Iterate all child objects in root object to find ILocalizable objects
                foreach (GameObject rootGameObject in rootGameObjects)
                {
                    GameObject[] childObjects = rootGameObject.GetComponentsInChildren<Transform>(true).Select(x => x.gameObject).ToArray();

                    foreach (GameObject childObject in childObjects)
                    {
                        if (childObject.TryGetComponent(out ILocalizable localizableGameObject))
                            gameObjectsInScenesReferenceList.Add(new(scene.name, localizableGameObject));
                    }
                }
            }

            // ? Closes all opened scenes (except that was originally opened)
            for (int i = SceneManager.sceneCountInBuildSettings - 1; i > 0; i--)
            {
                Scene scene = EditorSceneManager.GetSceneAt(i);

                if (scene.isLoaded)
                    EditorSceneManager.CloseScene(scene, true);
            }
        }

        private void FindReferencesInPrefabs()
        {
            referencesInPrefabsList = new();

            string[] prefabsGUIDs = AssetDatabase.FindAssets("t:Prefab");
            foreach (string guid in prefabsGUIDs)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath<Object>(prefabPath);

                // ? Checks parent object
                if (prefab.TryGetComponent(out ILocalizable localizablePrefab))
                    referencesInPrefabsList.Add(localizablePrefab);

                // ? Checks all child objects
                GameObject[] childObjects = prefab.GetComponentsInChildren<Transform>(true).Select(x => x.gameObject).ToArray();

                foreach (GameObject childObject in childObjects)
                {
                    if (childObject.TryGetComponent(out ILocalizable localizableChild))
                        referencesInPrefabsList.Add(localizableChild);
                }
            }
        }

        private void FindReferencesInScriptableObjects()
        {
            referencesInScriptableObjectsList = new();

            string[] objectsGUIDs = AssetDatabase.FindAssets("t:Object");
            foreach (string guid in objectsGUIDs)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

                if (asset is ILocalizable localizableAsset)
                    referencesInScriptableObjectsList.Add(localizableAsset);
            }
        }

        private void FindReferencesInScripts()
        {
            referencesInScriptsDictionary = new();

            foreach (RowData rowData in dataRows)
            {
                int totalReferencesAmount = 0;
                string[] scriptPaths = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);

                foreach (string scriptPath in scriptPaths)
                {
                    string scriptContent = File.ReadAllText(scriptPath);
                    int matchesCount = Regex.Matches(scriptContent, rowData.Key).Count;

                    if(matchesCount > 0)
                    {
                        totalReferencesAmount += matchesCount;

                        // ? If reference dictionary contains key - will add path to that list, otherwise will create new entry.
                        if(referencesInScriptsDictionary.ContainsKey(rowData.Key))
                        {
                            // ? Will add a path to script if it doesn't already exists
                            if(referencesInScriptsDictionary[rowData.Key].Contains(scriptPath) == false)
                                referencesInScriptsDictionary[rowData.Key].Add(scriptPath);
                        }
                        else
                            referencesInScriptsDictionary.Add(rowData.Key, new() { scriptPath } );
                    }
                }

                rowData.referencesInScripts = totalReferencesAmount;
            }
        }

        private void UpdateReferencesCount()
        {
            if (HasAccessToPopulatedDataBase())
            {
                foreach (RowData rowData in dataRows)
                {
                    rowData.referencesInGameObjects = gameObjectsInScenesReferenceList.Count(x => x.Value.HasLocalizationKey(rowData.Key));
                    rowData.referencesInPrefabs = referencesInPrefabsList.Count(x => x.HasLocalizationKey(rowData.Key));
                    rowData.referencesInScriptableObjects = referencesInScriptableObjectsList.Count(x => x.HasLocalizationKey(rowData.Key));
                }
            }
        }

        private void UpdateFields(RowData rowData)
        {
            if (HasAccessToPopulatedDataBase() == false)
                return;

            string message = $"Updated Key in";

            if (rowData.referencesInGameObjects > 0)
            {
                UpdateInGameObjects(rowData);
                message += $" {rowData.referencesInGameObjects} GameObject(s),";
            }

            if (rowData.referencesInPrefabs > 0)
            {
                UpdateInPrefabs(rowData);
                message += $" {rowData.referencesInPrefabs} Prefab(s),";
            }

            if (rowData.referencesInScriptableObjects > 0)
            {
                UpdateInScriptableObjects(rowData);
                message += $" {rowData.referencesInScriptableObjects} ScriptableObject(s),";
            }

            if (rowData.referencesInScripts > 0)
                UpdateInScripts(rowData);
            
            message = message.TrimEnd(',') + '.';
            Debug.Log(message);

            rowData.UpdateInitialKey();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            SaveChanges();
        }

        private void UpdateInGameObjects(RowData rowData)
        {
            foreach (KeyValuePair<string, ILocalizable> gameObjectReference in gameObjectsInScenesReferenceList)
            {
                if (gameObjectReference.Value.HasLocalizationKey(rowData.InitialKey))
                {
                    gameObjectReference.Value.ReplaceKey(rowData.InitialKey, rowData.Key);
                    EditorUtility.SetDirty((Object)gameObjectReference.Value);
                }
            }
        }
    
        private void UpdateInPrefabs(RowData rowData)
        {
            foreach (ILocalizable localizablePrefab in referencesInPrefabsList)
            {
                if (localizablePrefab.HasLocalizationKey(rowData.InitialKey))
                {
                    localizablePrefab.ReplaceKey(rowData.InitialKey, rowData.Key);

                    PrefabUtility.RecordPrefabInstancePropertyModifications((Object)localizablePrefab);
                }
            }
        }

        private void UpdateInScriptableObjects(RowData rowData)
        {
            foreach (ILocalizable localizableScriptableObject in referencesInScriptableObjectsList)
            {
                if (localizableScriptableObject.HasLocalizationKey(rowData.InitialKey))
                {
                    localizableScriptableObject.ReplaceKey(rowData.InitialKey, rowData.Key);
                    EditorUtility.SetDirty((ScriptableObject)localizableScriptableObject);
                }
            }
        }

        private void UpdateInScripts(RowData rowData)
        {
            KeyValuePair<string, List<string>> scriptPathsKVP = referencesInScriptsDictionary.First(x => x.Key == rowData.InitialKey);

            foreach (string pathToScript in scriptPathsKVP.Value)
            {
                try
                {
                    string fileContent = File.ReadAllText(pathToScript);
                    fileContent = fileContent.Replace(rowData.InitialKey, rowData.Key);
                    File.WriteAllText(pathToScript, fileContent);
                }
                catch (System.Exception exception)
                {
                    Debug.LogError("An error occurred: " + exception.Message);
                }
            }
        }

    #endregion

    #region Additional
        private bool HasAccessToPopulatedDataBase()
        {
            if (IsDataBaseUpdated() == false)
            {
                Debug.LogError("Please, update data base before getting references amount.");
                return false;
            }

            if (gameObjectsInScenesReferenceList.Count == 0 && referencesInPrefabsList.Count == 0 && referencesInScriptableObjectsList.Count == 0 && referencesInScriptsDictionary.Count == 0)
            {
                Debug.LogWarning("Data base is empty. Create any GameObject / ScriptableObject with ILocalizable interface to populate the data base.");
                return false;
            }

            return true;
        }

        private bool IsDataBaseUpdated()
        {
            return gameObjectsInScenesReferenceList != null&& referencesInPrefabsList != null && referencesInScriptableObjectsList != null  && referencesInScriptsDictionary != null;
        }
        
        private void ForceToSaveFile()
        {
            if (hasUnsavedChanges == false)
                hasUnsavedChanges = true;
        }

        private static GUIStyle CenteredStyle
        {
            get
            {
                GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
                centeredStyle.alignment = TextAnchor.UpperCenter;

                return centeredStyle;
            }
        }

        private static GUIStyle TextFieldStyle
        {
            get
            {
                GUIStyle textFieldStyle = GUI.skin.GetStyle("TextField");
                textFieldStyle.wordWrap = true;

                return textFieldStyle;
            }
        }
                
    #endregion

        private class RowData
        {
            public List<string> cells;
            public bool shouldBeDisplayed;
            public int referencesInGameObjects;
            public int referencesInPrefabs;
            public int referencesInScriptableObjects;
            public int referencesInScripts;

            public string InitialKey { get; private set; } 
            public string Key => cells.Count == 0? string.Empty : cells[0];
            public string KeyTag
            {
                get
                {
                    if (Key == string.Empty)
                    {
                        Debug.LogError("Key is empty! Returned an empty tag.");
                        return string.Empty;
                    }

                    return Key.Split('_')[0];
                }
            }

            public int HeightMultiplier
            {
                get
                {
                    int longestText = cells.Max(x => x.Length);
                    return (longestText / MAX_CHARS_IN_LINE) + 1;
                }
            }

            public RowData()
            {
                cells = new();
                shouldBeDisplayed = true;
                referencesInGameObjects = -1;
                referencesInScriptableObjects = -1;
                referencesInPrefabs = -1;
                referencesInScripts = -1;
            }

            public void UpdateInitialKey() => InitialKey = Key;
            public string GetRowDataForCSV() => $"\"{string.Join("\",\"", cells)}\"";

            public string GetGameObjectsReferenceAmount() => (referencesInGameObjects < 0)? "?" : referencesInGameObjects.ToString();
            public string GetPrefabsReferenceAmount() => (referencesInPrefabs < 0)? "?" : referencesInPrefabs.ToString();
            public string GetScriptableObjectsReferenceAmount() => (referencesInScriptableObjects < 0)? "?" : referencesInScriptableObjects.ToString();
            public string GetScriptsReferenceAmount() => (referencesInScripts < 0)? "?" : referencesInScripts.ToString();

            public bool HasReferenceInAnyObject() => HasReferencesInGameObjects() ||  HasReferencesInPrefabs() || HasReferencesInScriptableObjects() || HasReferencesInScripts();
            public bool HasReferencesInGameObjects() => referencesInGameObjects > 0;
            public bool HasReferencesInPrefabs() => referencesInPrefabs > 0;
            public bool HasReferencesInScriptableObjects() => referencesInScriptableObjects > 0;
            public bool HasReferencesInScripts() => referencesInScripts > 0;
        }
    }
}