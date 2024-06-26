using System;
using UnityEngine;

namespace CGames
{
    [Serializable]
    public class LocalizationKeyField
    {
        [field: SerializeField] public string LocalizationKey { get; private set; }

        public void UpdateKey(string newKey) => LocalizationKey = newKey;
    }
}