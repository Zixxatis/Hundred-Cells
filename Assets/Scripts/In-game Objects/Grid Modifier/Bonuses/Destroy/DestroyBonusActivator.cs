using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class DestroyBonusActivator : BonusActivator
    {
        public override BonusType BonusType => BonusType.Destroy;
        public override bool ShouldBlockGrid => false;
        public override bool ShouldBlockHand => true;
        
        [Header("Bonus Specific Elements")]
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private DestructionShape destructionShape;

        private Func<bool> isAnyCellOnGrid;
        private IDimensionsNotifier dimensionsNotifier;

        [Inject]
        private void Construct(GameplayGrid gameplayGrid, IDimensionsNotifier dimensionsNotifier)
        {
            this.isAnyCellOnGrid = () => gameplayGrid.HasAnyColoredCell;
            this.dimensionsNotifier = dimensionsNotifier;
        }

        protected override void InitializeObject()
        {
            destructionShape.Initialize(this.transform, Deactivate);
        }

        public override void Activate()
        {
            base.Activate();

            AdjustSizes();

            if(isAnyCellOnGrid.Invoke() == false)
                Deactivate();
        }

        private void AdjustSizes()
        {
            gridLayoutGroup.cellSize = dimensionsNotifier.CellDimensions;
            gridLayoutGroup.spacing = dimensionsNotifier.GridSpacing;
        }
    }
}