using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    [RequireComponent(typeof(Graphic))]
    public class ColorMimic : ColorAdapter
    {
        [SerializeField] private CellColor mimicColor;

        // private Graphic graphic;
        // private CollectionsInventory collectionsInventory;

        // [Inject]
        // private void Construct(CollectionsInventory collectionsInventory)
        // {
        //     this.collectionsInventory = collectionsInventory;
        // }

        // private void Start()
        // {
        //     UpdateColor();
        //     collectionsInventory.OnCollectionChanged += UpdateColor;
        // }

        public void ChangeColor(CellColor cellColor)
        {
            mimicColor = cellColor;
            UpdateColor();
        }

        protected override void UpdateColor()
        {
            // if(graphic == null)
            //     graphic = GetComponent<Graphic>();

            Color newColor = CollectionsInventory.SelectedCollection.GetColor32ForCell(mimicColor);
            Graphic.color = new(newColor.r, newColor.g, newColor.b, Graphic.color.a);
        }

        // private void OnDestroy()
        // {
        //     collectionsInventory.OnCollectionChanged -= UpdateColor;
        // }
    }
}