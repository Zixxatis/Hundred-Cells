using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CGames.CustomEditors
{
    public static class ProjectSetup3R
    {
        [MenuItem("Tools/Setup/Create Default Folders")]
        public static void CreateDefaultFolders()
        {
            List<string> foldersNameList = new()
            {
                "Audio", "Animations", "Fonts", "Plugins", "Prefabs", "Resources", "Scenes", "Scripts", "Sprites", 
            };

            List<string> editorSubFoldersList = new()
            {
                "Context Menu Tools", "GameObject Creation", "Inspector GUI", "Windows" 
            };

            foreach (string folderName in foldersNameList)
            {
                Directory.CreateDirectory(Path.Combine(Application.dataPath, folderName));
            }

            foreach (string subFolderName in editorSubFoldersList)
            {
                Directory.CreateDirectory(Path.Combine(Application.dataPath, "Editor", subFolderName));
            }
            
            AssetDatabase.Refresh();

            if(!Directory.Exists(Path.Combine(Application.dataPath, "TextMesh Pro")))
                Debug.LogWarning("Don't forget to manually import TMP Essentials!");
        }
    }
}