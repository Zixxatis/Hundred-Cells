using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace CGames
{
    public abstract class Data
    {
        [JsonIgnore] public abstract byte Version { get; }
        [JsonIgnore] public abstract string LogPrefix { get; }
        [JsonIgnore] protected abstract string FileName { get; }

        [JsonIgnore] public string FullPath => Path.Combine(Application.persistentDataPath, FileName);
    }
}