using UnityEngine;

namespace CGames
{
    public class RotateBonusActivator : BonusActivator
    {
        public override BonusType BonusType => BonusType.Rotate;
        public override bool ShouldBlockGrid => true;
        public override bool ShouldBlockHand => false;

        [Header("Bonus Specific Elements")]
        [SerializeField] private RotateBonusCather rotateLeftCather;
        [SerializeField] private RotateBonusCather rotateRightCather;

        protected override void InitializeObject()
        {
            rotateLeftCather.Initialize(() => Deactivate());
            rotateRightCather.Initialize(() => Deactivate());
        }
    }
}