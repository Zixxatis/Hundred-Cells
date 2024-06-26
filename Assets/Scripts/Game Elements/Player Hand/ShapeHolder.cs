using UnityEngine;

namespace CGames
{
    public class ShapeHolder : MonoBehaviour
    {
        public Shape ShapeInSlot { get; private set; }
        private float? nonDefaultScale;

        public bool HasShape => ShapeInSlot != null;
        public bool IsEmpty => ShapeInSlot == null;

        public void BindShapeToHolder(Shape shape, float? nonDefaultScale = null)
        {
            this.ShapeInSlot = shape;
            this.nonDefaultScale = nonDefaultScale;

            shape.ShapeDragHandler.BindToShapeHolder(ReturnShapeToHand, ClearHolder);
            ReturnShapeToHand();
        }

        private void ReturnShapeToHand()
        {
            ShapeInSlot.ActivateGameObject();
            ShapeInSlot.transform.SetParent(this.transform);

            if(nonDefaultScale == null)
                ShapeInSlot.SetScale(GameConstants.ShapeHolderDefaultScale);
            else
                ShapeInSlot.SetScale((float)nonDefaultScale);
        }

        private void ClearHolder() => ShapeInSlot = null;

        public void UnbindShape()
        {
            if(HasShape)
            {
                ShapeInSlot.ReleaseShape();
                ClearHolder();
            }
        }
    }
}