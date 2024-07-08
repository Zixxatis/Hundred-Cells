using UnityEngine.UI;

namespace CGames
{
    public class BackgroundImage : ColorAdapter
    {
        protected override void UpdateColor()
        {
            Graphic.color = CollectionsInventory.SelectedCollection.BackgroundColor;
        }
    }
}