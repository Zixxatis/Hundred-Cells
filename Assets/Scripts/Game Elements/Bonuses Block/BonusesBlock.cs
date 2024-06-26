using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class BonusesBlock : MonoBehaviour
    {
        [SerializeField] private List<BonusSlot> bonusSlotsList;

        private SavingSystem savingSystem;
        private BonusSystem bonusSystem;
        private IShapePlacedNotifier shapePlacedNotifier;
        private INewGameStartedNotifier newGameStartedNotifier;

        [Inject]
        private void Construct(BonusSystem bonusSystem, SavingSystem savingSystem, IShapePlacedNotifier shapePlacedNotifier, INewGameStartedNotifier newGameStartedNotifier)
        {
            this.savingSystem = savingSystem;
            this.bonusSystem = bonusSystem;
            this.shapePlacedNotifier = shapePlacedNotifier;
            this.newGameStartedNotifier = newGameStartedNotifier;
        }

        private void Awake()
        {
            bonusSystem.OnBonusDataChanged += SavePlayerAndSessionData;
            shapePlacedNotifier.OnShapePlaced += AllowToUseBonuses;
            newGameStartedNotifier.OnNewGameStarted += ForbidToUseBonuses;

            InitializeSlots();
        }

        private void InitializeSlots()
        {
            List<BonusType> bonusTypes = Enum.GetValues(typeof(BonusType)).Cast<BonusType>().ToList();

            for (int i = 0; i < bonusTypes.Count; i++)
            {
                bonusSlotsList[i].Initialize(bonusTypes[i], ForbidToUseBonuses);
            }
            
            if(bonusSystem.CanUseBonusesThisTurn)
                AllowToUseBonuses();
            else
                ForbidToUseBonuses();
        }

        private void SavePlayerAndSessionData()
        {
            savingSystem.SaveData<PlayerData>();
            savingSystem.SaveData<SessionData>();
        }

        private void AllowToUseBonuses()
        {
            bonusSystem.AllowToUseBonuses();
            bonusSlotsList.ForEach(x => x.TryToEnableInteractivity());
        }

        private void ForbidToUseBonuses()
        {
            bonusSystem.ForbidToUseBonuses();
            bonusSlotsList.ForEach(x => x.DisableInteractivity());
        }

        private void OnDestroy()
        {
            bonusSystem.OnBonusDataChanged -= SavePlayerAndSessionData;
            shapePlacedNotifier.OnShapePlaced -= AllowToUseBonuses;
            newGameStartedNotifier.OnNewGameStarted -= ForbidToUseBonuses;
        }

    #if UNITY_EDITOR
        [ContextMenu("Fill List")]
        private void FillList()
        { 
            bonusSlotsList = new();

            foreach (Transform child in this.transform)
            {
                if(child.TryGetComponent(out BonusSlot bonusSlot))
                    bonusSlotsList.Add(bonusSlot);
            }
            
            if(bonusSlotsList.Count != Enum<BonusType>.Length)
                Debug.LogWarning($"List doesn't match expected number of elements! ({bonusSlotsList.Count} / {Enum<BonusType>.Length})");
            else
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
    #endif
    }
}