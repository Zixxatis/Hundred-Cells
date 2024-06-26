using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    [RequireComponent(typeof(Image))]
    public class Cell : MonoBehaviour
    {
        [field: SerializeField] public CellColor CellColor { get; private set; }
        public bool IsFilled => CellColor != CellColor.None;

        private CollectionsInventory collectionsInventory;
        private Image image;

        [Inject]
        private void Construct(CollectionsInventory collectionsInventory)
        {
            this.collectionsInventory = collectionsInventory;
        }

        private void Awake()
        {
            collectionsInventory.OnCollectionChanged += RefreshColor;
        }

        public void UpdateColor(CellColor cellColor)
        {
            CellColor = cellColor;
            RefreshColor();
        }

        private void RefreshColor()
        {
            if(image == null)
                image = GetComponent<Image>();

            image.color = collectionsInventory.SelectedCollection.GetColor32ForCell(CellColor);
        }

        public void ClearCell() => UpdateColor(CellColor.None);

        private void OnDestroy()
        {
            collectionsInventory.OnCollectionChanged -= RefreshColor;
        }
    }
}