using System;
using Zenject;

namespace CGames
{
    public class RevertBonusActivator : BonusActivator
    {
        public override BonusType BonusType => BonusType.Revert;
        public override bool ShouldBlockGrid => false;
        public override bool ShouldBlockHand => false;

        private Action<SessionData> overrideSessionData;
        private ISnapshotLoader snapshotLoader;

        [Inject]
        private void Construct(SavingSystem savingSystem, ISnapshotLoader snapshotLoader)
        {
            this.overrideSessionData = savingSystem.OverrideData;
            this.snapshotLoader = snapshotLoader;
        }
        
        protected override void InitializeObject() { }

        public override void Activate()
        {
            base.Activate();

            if(snapshotLoader.HasCapturedData())
                overrideSessionData.Invoke(snapshotLoader.GetDataFromSnapshot());

            Deactivate();
        }
    }
}