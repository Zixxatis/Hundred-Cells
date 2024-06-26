using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace CGames
{
    public class SettingsMP : MenuPanel
    {
        [Header("Setting Slots")]
        [SerializeField] private PlayerNameSS playerNameSS;
        [SerializeField] private LocalizationSS localizationSS;
        [SerializeField] private MusicSS musicSS;
        [SerializeField] private EffectsSS effectsSS;
        [SerializeField] private DraggingDistanceSS draggingDistanceSS;

        private SavingSystem savingSystem;
        private List<SettingSlot> settingSlotsList;

        [Inject]
        private void Construct(SavingSystem savingSystem)
        {
            this.savingSystem = savingSystem;
        }

        protected override void Awake()
        {
            base.Awake();

            settingSlotsList = new()
            {
                playerNameSS,
                localizationSS,
                musicSS,
                effectsSS,
                draggingDistanceSS
            };

            settingSlotsList.ForEach(x => x.PrepareSettingSlot());
        }

        public override void PrepareBeforeOpening()
        {
            settingSlotsList.ForEach(x => x.MatchValuesToCurrent());
        }

        public override void PrepareBeforeClosing() => savingSystem.SaveData<ConfigData>();
    }
}