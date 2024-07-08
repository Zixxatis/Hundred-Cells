using UnityEngine;

namespace CGames
{    
    [CreateAssetMenu(fileName = "Encryption Config", menuName = "Configs/Encryption Config", order = 0)]
    public class EncryptionConfig : ScriptableObject
    {
        [field: Header("Behaviour")]
        [field: SerializeField] public bool ShouldEncrypt { get; private set; }
        [field: SerializeField] public bool ShouldDecrypt { get; private set; }

        [Header("Current Code Word")]
        [SerializeField, InspectorReadOnly] private string codeWord = string.Empty;

        public string CodeWord => codeWord;
        
        /// <summary> Used to set a code word via Reflection. </summary>        
        private void SetCodeWord(string newCode) => codeWord = newCode;
    }
}