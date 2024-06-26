using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class BackgroundImage : ColorAdapter
    {
        // private Image image;
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

        protected override void UpdateColor()
        {
            // if(image == null)
            //     image = GetComponent<Image>();

            Graphic.color = CollectionsInventory.SelectedCollection.BackgroundColor;
        }

        // private void OnDestroy()
        // {
        //     collectionsInventory.OnCollectionChanged -= UpdateColor;
        // }
    }
}