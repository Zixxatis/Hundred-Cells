using System;
using Zenject;

namespace CGames
{
    public class RefreshBonusActivator : BonusActivator
    {
        public override BonusType BonusType => BonusType.Refresh;
        public override bool ShouldBlockGrid => false;
        public override bool ShouldBlockHand => false;

        private Action rerollHandAction;

        [Inject]
        private void Construct(PlayerHand playerHand)
        {
            this.rerollHandAction = playerHand.ClearHandAndRespawnRandomly;
        }

        protected override void InitializeObject() { }

        public override void Activate()
        {
            base.Activate();

            rerollHandAction.Invoke();

            Deactivate();
        }
    }
}