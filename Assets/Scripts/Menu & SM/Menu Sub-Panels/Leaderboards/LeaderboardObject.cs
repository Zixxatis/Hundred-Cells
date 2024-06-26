using System;
using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    public class LeaderboardObject : MonoBehaviour
    {
        [SerializeField] private TextLocalizer headerLTMP;
        [Space]
        [SerializeField] private Transform contentTransform;
        [SerializeField] private List<LeaderboardRecordObject> recordsList;

        public GameMode GameMode { get; private set; }
        private Func<List<LeaderboardRecord>> getRecordsList;

        public void Initialize(GameMode gameMode, Func<List<LeaderboardRecord>> getRecordsList)
        {
            this.GameMode = gameMode;
            this.getRecordsList = getRecordsList;

            ChangeHeaderText();
        }

        private void ChangeHeaderText()
        {
            string key = GameMode switch
            {
                GameMode.Classic => "Leaderboards_Classic",
                GameMode.Infinite => "Leaderboards_Infinite",
                _ => throw new ArgumentOutOfRangeException(),
            };

            headerLTMP.SetKeyAndUpdate(key);
        }

        public void UpdateLeaderboard()
        {
            List<LeaderboardRecord> leaderboardRecordsList = getRecordsList.Invoke();

            for (int i = 0; i < leaderboardRecordsList.Count; i++)
            {
                recordsList[i].ChangeRecordText(i + 1, leaderboardRecordsList[i].PlayerName, leaderboardRecordsList[i].Score);
            }
        }

        public void ResetPosition() => contentTransform.localPosition = new(contentTransform.localPosition.x, 0);

        #if UNITY_EDITOR
        [ContextMenu("Fill List")]
        private void FillList()
        {   
            if(contentTransform == null)
                return;

            recordsList = new();

            foreach (Transform child in contentTransform)
            {
                if(child.TryGetComponent(out LeaderboardRecordObject recordObject))
                    recordsList.Add(recordObject);
            }
            
            if(recordsList.Count != LeaderboardsUtilities.LeaderboardPlayerCapacity)
                Debug.LogWarning($"List doesn't match expected number of elements! ({recordsList.Count} / {LeaderboardsUtilities.LeaderboardPlayerCapacity})");
            else
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
        #endif
    }
}