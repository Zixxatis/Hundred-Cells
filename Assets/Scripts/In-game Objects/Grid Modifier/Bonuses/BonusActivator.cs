using System;
using UnityEngine;
using Zenject;

namespace CGames
{
    public abstract class BonusActivator : MonoBehaviour
    {
        public abstract BonusType BonusType { get; }
        public abstract bool ShouldBlockGrid { get; }
        public abstract bool ShouldBlockHand { get; }

        protected Cell[,] cellsMatrix;
        private Action onBonusDeactivateAction;
        private Action validateGameStatusAction;
        private Action playSoundEffectAction;

        protected virtual void Awake() => this.DeactivateGameObject();

        [Inject]
        private void Construct(GridMediator gridMediator, AudioPlayer audioPlayer, ResourceSystem resourceSystem)
        {
            this.validateGameStatusAction = gridMediator.ValidateGameStatus;
            this.playSoundEffectAction = () => audioPlayer.PlaySFX(resourceSystem.BonusesRSS.GetBonusInfo(BonusType).EffectAudioClip);
        }

        public void Initialize(Cell[,] cellsMatrix, Action onBonusDeactivateAction)
        {
            this.cellsMatrix = cellsMatrix;
            this.onBonusDeactivateAction = onBonusDeactivateAction;

            InitializeObject();
        }

        protected abstract void InitializeObject();

        public virtual void Activate()
        {
            this.ActivateGameObject();
        }

        protected virtual void Deactivate()
        {
            playSoundEffectAction.Invoke();

            this.DeactivateGameObject();

            onBonusDeactivateAction?.Invoke();
            validateGameStatusAction?.Invoke();
        }
    }
}