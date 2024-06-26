using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class TransformBonusActivator : BonusActivator
    {
        public override BonusType BonusType => BonusType.Transform;
        public override bool ShouldBlockGrid => true;
        public override bool ShouldBlockHand => false;

        [Header("Bonus Specific Elements")]
        [SerializeField] private TransformBonusShapeSlot leftTransformBonusShapeSlot;
        [SerializeField] private TransformBonusShapeSlot middleTransformBonusShapeSlot;
        [SerializeField] private TransformBonusShapeSlot rightTransformBonusShapeSlot;

        private List<TransformBonusShapeSlot> transformBonusShapeSlotsList;
        private Action<List<ShapeData>> fillHandWithShapeAction;

        [Inject]
        private void Construct(PlayerHand playerHand)
        {
            this.fillHandWithShapeAction = playerHand.ClearHandAndRespawnCertainShapes;
        }

        protected override void InitializeObject()
        {
            transformBonusShapeSlotsList = new()
            {
                leftTransformBonusShapeSlot, middleTransformBonusShapeSlot, rightTransformBonusShapeSlot
            };

            transformBonusShapeSlotsList.ForEach(x => x.Initialize((y) => SelectShape(y))); 
        }

        public override void Activate()
        {
            base.Activate();

            List<ShapeType> randomShapeTypes = ShapesRandomizer.GetThreeUniqueShapeTypes();

            for (int i = 0; i < randomShapeTypes.Count; i++)
            {
                transformBonusShapeSlotsList[i].ShowShape(randomShapeTypes[i]);
            }
        }

        private void SelectShape(ShapeData shapeData)
        {
            fillHandWithShapeAction.Invoke
            (
                new List<ShapeData>
                {
                    shapeData, shapeData, shapeData
                }
            );
            
            transformBonusShapeSlotsList.ForEach(x => x.ReleaseShape());

            Deactivate();
        }
    }
}