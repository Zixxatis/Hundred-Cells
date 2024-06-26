using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CGames
{
    [RequireComponent(typeof(Image))]
    public class RotateBonusCather : MonoBehaviour, IDropHandler
    {
        [SerializeField] private RectTransform iconRT;
        [Space]
        [SerializeField] private bool shouldRotateToLeft;

        private Action deactivateAction;

        public void Initialize(Action deactivateAction)
        {
            this.deactivateAction = deactivateAction;

            if(shouldRotateToLeft == false)
                iconRT.localScale = new(-1, 1, 1);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if(eventData.pointerDrag.TryGetComponent(out Shape shape) == false)
                return;

            if(shape.IsRotatable)
            {                
                if(shouldRotateToLeft)
                    shape.ChangeRotationDegree(RotationDegreesUtilities.GetNextRotationDegrees(shape.RotationDegrees));
                else
                    shape.ChangeRotationDegree(RotationDegreesUtilities.GetPreviousRotationDegrees(shape.RotationDegrees));
            }

            deactivateAction.Invoke();
        }
    }
}