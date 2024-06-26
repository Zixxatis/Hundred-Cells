using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    [RequireComponent(typeof(Graphic))]
    public class ColorSchemeAdapter : ColorAdapter
    {
        [Tooltip("If selected - will use inverted color to selected theme. For example: on Dark Theme will set a Light color.")]
        [SerializeField] private bool useInvertedColors = true;

        //private Graphic graphic;
        private Func<bool, Color> getColor;
        //private CollectionsInventory collectionsInventory;

        [Inject]
        private void Construct(ResourceSystem resourceSystem)  //(CollectionsInventory collectionsInventory, ResourceSystem resourceSystem)
        {
            //this.collectionsInventory = collectionsInventory;
            this.getColor = resourceSystem.ColorsRSS.GetColorForGraphicElement;
        }

        // private void Start()
        // {
        //     UpdateColor();
        //     collectionsInventory.OnCollectionChanged += UpdateColor;
        // }

        protected override void UpdateColor()
        {
            // if(graphic == null)
            //     graphic = GetComponent<Graphic>();

            Color newColor; 
            
            if(useInvertedColors)
                newColor = getColor.Invoke(!CollectionsInventory.SelectedCollection.UsesLightTheme);
            else
                newColor = getColor.Invoke(CollectionsInventory.SelectedCollection.UsesLightTheme);

            Graphic.color = new(newColor.r, newColor.g, newColor.b, Graphic.color.a);
        }

        // private void OnDestroy()
        // {
        //     collectionsInventory.OnCollectionChanged -= UpdateColor;
        // }
    }
}