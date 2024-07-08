using UnityEngine;
using UnityEngine.UI;

namespace CGames
{
    [RequireComponent(typeof(Graphic))]
    public class ColorMimic : ColorAdapter
    {
        [SerializeField] private CellColor mimicColor;

        public void ChangeColor(CellColor cellColor)
        {
            mimicColor = cellColor;
            UpdateColor();
        }

        protected override void UpdateColor()
        {
            Color newColor = CollectionsInventory.SelectedCollection.GetColor32ForCell(mimicColor);
            Graphic.color = new(newColor.r, newColor.g, newColor.b, Graphic.color.a);
        }
    }
}