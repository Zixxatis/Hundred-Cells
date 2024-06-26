using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    [RequireComponent(typeof(Image))]
    public class ShapeCell : MonoBehaviour
    {
        private CollectionsInventory collectionsInventory;

        private Image image;
        private RectTransform rectTransform;

        private CellColor cellColor;
        
        [Inject]
        private void Construct(CollectionsInventory collectionsInventory)
        {
            this.collectionsInventory = collectionsInventory;
        }

        private void Awake()
        {
            collectionsInventory.OnCollectionChanged += RefreshColor;

            image = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
        }

        public void UpdateColor(CellColor cellColor)
        {
            this.cellColor = cellColor;
            RefreshColor();
        }

        private void RefreshColor()
        {
            image.color = collectionsInventory.SelectedCollection.GetColor32ForCell(cellColor);
        }

        public Cell FindCellBelow()
        {
            PointerEventData pointerData = new(EventSystem.current)
            {
                position = RectTransformUtility.WorldToScreenPoint(Camera.main, rectTransform.position)
            };

            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(pointerData, results);

            return results.Select(x => x.gameObject.GetComponent<Cell>())
                          .FirstOrDefault(component => component != null);
        }

        private void OnDestroy()
        {
            collectionsInventory.OnCollectionChanged -= RefreshColor;
        }
    }
}