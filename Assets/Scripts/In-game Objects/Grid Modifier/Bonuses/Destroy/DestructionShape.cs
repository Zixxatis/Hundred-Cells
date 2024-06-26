using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CGames
{
    public class DestructionShape : DragHandlerBase
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image mainImage;
        
        private Action deactivateAction;
        
        public void Initialize(Transform parentTransform, Action deactivateAction)
        {
            this.deactivateAction = deactivateAction;
            this.returnShapeToHand = () => this.transform.SetParent(parentTransform);
        }
       
        public override void OnEndDrag(PointerEventData eventData)
        {
            if(TryToFindCellBelow(out Cell cellBelow))
            {
                cellBelow.ClearCell();
                deactivateAction.Invoke();
            }

            base.OnEndDrag(eventData);
        }

        private bool TryToFindCellBelow(out Cell cellBelow)
        {
            PointerEventData pointerData = new(EventSystem.current)
            {
                position = RectTransformUtility.WorldToScreenPoint(Camera.main, RectTransform.position)
            };

            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(pointerData, results);

            cellBelow = results.Select(x => x.gameObject.GetComponent<Cell>())
                          .FirstOrDefault(component => component != null);

            return cellBelow != null && cellBelow.IsFilled;
        }

        public override void EnableRaycast()
        {
            backgroundImage.raycastTarget = true;
            mainImage.raycastTarget = true;
        }

        protected override void DisableRaycast()
        {
            backgroundImage.raycastTarget = false;
            mainImage.raycastTarget = false;
        }
    }
}