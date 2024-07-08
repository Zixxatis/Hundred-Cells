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

        private Func<bool, Color> getColor;

        [Inject]
        private void Construct(ResourceSystem resourceSystem)
        {
            this.getColor = resourceSystem.ColorsRSS.GetColorForGraphicElement;
        }

        protected override void UpdateColor()
        {
            Color newColor; 
            
            if(useInvertedColors)
                newColor = getColor.Invoke(!CollectionsInventory.SelectedCollection.UsesLightTheme);
            else
                newColor = getColor.Invoke(CollectionsInventory.SelectedCollection.UsesLightTheme);

            Graphic.color = new(newColor.r, newColor.g, newColor.b, Graphic.color.a);
        }
    }
}