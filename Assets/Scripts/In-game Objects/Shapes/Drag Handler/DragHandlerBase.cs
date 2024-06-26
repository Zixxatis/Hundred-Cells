using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace CGames
{
    public abstract class DragHandlerBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        protected RectTransform RectTransform { get; private set; }
        protected Action returnShapeToHand;

        private DragCanvas dragCanvas;
        private Func<float> getCurrentDraggingHeightValue;

        [Inject]
        private void Construct(DragCanvas dragCanvas, PlayerPreferences playerPreferences)
        {
            this.dragCanvas = dragCanvas;
            this.getCurrentDraggingHeightValue = () => playerPreferences.CurrentDraggingHeightValue;
        }

        protected virtual void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            RectTransform.localPosition = new(RectTransform.localPosition.x, RectTransform.localPosition.y + getCurrentDraggingHeightValue.Invoke());
            dragCanvas.MoveToDraggingZone(RectTransform);
            DisableRaycast();
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            Vector2 adjustedDelta = eventData.delta / dragCanvas.Canvas.scaleFactor;
            RectTransform.anchoredPosition += adjustedDelta;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            EnableRaycast();
            returnShapeToHand.Invoke();
        }

        public abstract void EnableRaycast();
        protected abstract void DisableRaycast();
    }
}