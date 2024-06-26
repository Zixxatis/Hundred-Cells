using UnityEngine;
using UnityEngine.UI;

namespace CGames
{
    [RequireComponent(typeof(Image))]
    public class ColorPreviewObject : MonoBehaviour
    {
        [field: SerializeField] public CellColor CellColor { get; private set; }
        
        private Image image;

        public void SetColor(Color32 newColor)
        {
            if(image == null)
                image = GetComponent<Image>();

            image.color = newColor;
        }
    }
}