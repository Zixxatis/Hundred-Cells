using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class ShapeDragHandler : DragHandlerBase
    {
        [SerializeField] private Image raycastImage;

        private IGridMediatorActivator gridMediatorActivator;
        private Action clearHolderAction;

        [Inject]
         private void Construct(IGridMediatorActivator gridMediatorActivator)
        {
            this.gridMediatorActivator = gridMediatorActivator;
            GetComponent<Shape>().AssignUnbindAction(UnbindFromShapeHolder);
        }

        public void BindToShapeHolder(Action returnShapeToHand, Action clearHolderAction)
        {
            this.returnShapeToHand = returnShapeToHand;
            this.clearHolderAction = clearHolderAction;
        }

        private void UnbindFromShapeHolder() => clearHolderAction.Invoke();

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            gridMediatorActivator.EnableGridMediatorRaycast();
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            gridMediatorActivator.DisableGridMediatorRaycast();
        }

        public override void EnableRaycast() => raycastImage.raycastTarget = true;
        protected override void DisableRaycast() => raycastImage.raycastTarget = false;
    }
}