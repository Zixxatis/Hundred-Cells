using System;
using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    public class SavingSystem : IEarlyInitializable, IPausable, IQuitable
    {
        private readonly ISaver<ConfigData> configDataSaver;
        private readonly ISaver<PlayerData> playerDataSaver;
        private readonly ISaver<RecordsData> recordsDataSaver;
        private readonly ISaver<SessionData> sessionDataSaver;

        private readonly Dictionary<Type, Action> saveActionsDictionary;
        private readonly Dictionary<Type, Action<Data>> overrideActionsDictionary;

        public SavingSystem(List<ISavable<ConfigData>> configDataSaver, List<ISavable<PlayerData>> playerDataSaver, 
                            List<ISavable<RecordsData>> recordsDataSaver, List<ISavable<SessionData>> sessionDataSaver)
        {
            this.configDataSaver = new DataSaver<ConfigData>(configDataSaver);
            this.playerDataSaver = new DataSaver<PlayerData>(playerDataSaver);
            this.recordsDataSaver = new DataSaver<RecordsData>(recordsDataSaver);
            this.sessionDataSaver = new DataSaver<SessionData>(sessionDataSaver);

            saveActionsDictionary = GetSaveActionsDictionary();
            overrideActionsDictionary = GetOverrideActionsDictionary();
        }

        private Dictionary<Type, Action> GetSaveActionsDictionary()
        {
            return new Dictionary<Type, Action>
            {
                { typeof(ConfigData), () => configDataSaver.SaveData() },
                { typeof(PlayerData), () => playerDataSaver.SaveData() },
                { typeof(SessionData), () => sessionDataSaver.SaveData() },
                { typeof(RecordsData), () => recordsDataSaver.SaveData() }
            };
        }

        private Dictionary<Type, Action<Data>> GetOverrideActionsDictionary()
        {
            return new Dictionary<Type, Action<Data>>
            {
                { typeof(ConfigData), (x) => configDataSaver.OverrideData((ConfigData)x) },
                { typeof(PlayerData), (x) => playerDataSaver.OverrideData((PlayerData)x) },
                { typeof(RecordsData), (x) => recordsDataSaver.OverrideData((RecordsData)x) },
                { typeof(SessionData), (x) => sessionDataSaver.OverrideData((SessionData)x) }
            };
        }

        void IEarlyInitializable.InitializeEarly()
        {
            configDataSaver.LoadData();
            playerDataSaver.LoadData();
            recordsDataSaver.LoadData();
            sessionDataSaver.LoadData();
        }

        /// <summary> Saves all data from scripts that implement ISavable interface of "T" type. </summary>
        public void SaveData<T>() where T : Data
        {
            Type type = typeof(T);

            if (saveActionsDictionary.TryGetValue(type, out var saveAction))
                saveAction.Invoke();
            else
                throw new InvalidOperationException($"Unsupported data type: {type}. Make sure this type is added to \"saveActionsDictionary\".");
        }

        public void OverrideData<T>(T data) where T : Data
        {
            Type type = typeof(T);

            if (overrideActionsDictionary.TryGetValue(type, out var overrideAction))
                overrideAction.Invoke(data);
            else
                throw new InvalidOperationException($"Unsupported data type: {type}. Make sure this type is added to \"overrideActionsDictionary\".");
        }

        void IPausable.OnPauseStarted()
        {
            if(Application.platform == RuntimePlatform.Android)
            {
                SaveData<PlayerData>();
                SaveData<SessionData>();
            }
        }

        void IPausable.OnPauseFinished(){ }

        void IQuitable.PrepareToQuit()
        {
            SaveData<PlayerData>();
            SaveData<SessionData>();
        }
    }
}