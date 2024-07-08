using System.Text;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace CGames.CustomEditors
{
    public class CodeGenerationWindow : EditorWindow
    {
        private int codeWordLength = 20;
        private bool shouldUseUpperCase = true;
        private bool shouldUseLowerCase = true;
        private bool shouldUseNumbers = true;
        private bool shouldUseSpecialCharacters = true;
        private bool shouldOverwriteAnyway = false;

        private EncryptionConfig encryptionConfig;

        public static void ShowWindow(EncryptionConfig config)
        {
            CodeGenerationWindow window = GetWindow<CodeGenerationWindow>("Code Generation");
            window.encryptionConfig = config;
        }

        private void OnGUI()
        {
            DrawPreferenceElements();
            GUILayout.Space(20);
            DrawButtons();
        }

        private void DrawPreferenceElements()
        {
            GUILayout.Label("Please, specify code-word preferences.", EditorStyles.boldLabel);

            GUILayout.Label("Enter code word length:");
            codeWordLength = EditorGUILayout.IntField(codeWordLength);

            GUILayout.Label("Should code-word include (A-Z) symbols?");
            GUI.enabled = false;
            shouldUseUpperCase = EditorGUILayout.Toggle(shouldUseUpperCase);
            GUI.enabled = true;

            GUILayout.Label("Should code-word include (a-z) symbols?");
            shouldUseLowerCase = EditorGUILayout.Toggle(shouldUseLowerCase);

            GUILayout.Label("Should code-word include (0-9) symbols?");
            shouldUseNumbers = EditorGUILayout.Toggle(shouldUseNumbers);

            GUILayout.Label("Should code-word include (!@#...) symbols?");
            shouldUseSpecialCharacters = EditorGUILayout.Toggle(shouldUseSpecialCharacters);
        }
        
        private void DrawButtons()
        {
            if(string.IsNullOrEmpty(encryptionConfig.CodeWord) == false)
            {
                GUIStyle wordWrappedLabelStyle = new(EditorStyles.boldLabel)
                {
                    alignment = TextAnchor.MiddleCenter,
                    wordWrap = true
                };

                GUILayout.Label("Warning! Overwriting an existing codeword will cause all previously encrypted saved files to be decrypted incorrectly!", wordWrappedLabelStyle);

                GUILayout.Label("Overwrite anyway?");
                shouldOverwriteAnyway = EditorGUILayout.Toggle(shouldOverwriteAnyway);

                if(shouldOverwriteAnyway)
                    DrawGenerateButton();
            }
            else
                DrawGenerateButton();
        }

        private void DrawGenerateButton()
        {
            MethodInfo methodInfo = encryptionConfig.GetType().GetMethod("SetCodeWord", BindingFlags.NonPublic | BindingFlags.Instance);

            if (GUILayout.Button("Generate"))
            {
                string codeWord = GenerateCodeWord();
                methodInfo.Invoke(encryptionConfig, new object[] { codeWord });
                EditorUtility.SetDirty(encryptionConfig);
            }
        }

        private string GenerateCodeWord()
        {
            string upperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string lowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
            string numberCharacters = "0123456789";
            string specialCharacters = "!@#$%^&*()-_=+[]{}|;:,.<>?";

            StringBuilder characterSetSB = new();

            if(shouldUseUpperCase)
                characterSetSB = characterSetSB.Append(upperCaseLetters);

            if(shouldUseLowerCase)
                characterSetSB = characterSetSB.Append(lowerCaseLetters);

            if(shouldUseNumbers)
                characterSetSB = characterSetSB.Append(numberCharacters);

            if(shouldUseSpecialCharacters)
                characterSetSB = characterSetSB.Append(specialCharacters);

            if(characterSetSB.Length == 0)
                throw new System.ArgumentException("At least one type of characters must be selected.");

            StringBuilder codeWordSB = new (codeWordLength);

            for (int i = 0; i < codeWordLength; i++)
            {
                int randomCharacterIndex = Random.Range(0, characterSetSB.Length);
                codeWordSB.Append(characterSetSB[randomCharacterIndex]);
            }

            return codeWordSB.ToString();
        }
    }
}