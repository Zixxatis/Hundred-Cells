using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    [RequireComponent(typeof(RawImage))]
    public class PatternScroller : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField, Range(0, 5)] private float scrollSpeed = 0.15f;
        [Space]
        [SerializeField, Range(-1, 1)] private int xDirection = 1;
        [SerializeField, Range(-1, 1)] private int yDirection = 1;

        private RawImage rawImage;
        private CollectionsInventory collectionsInventory;

        [Inject]
        private void Construct(CollectionsInventory collectionsInventory)
        {
            this.collectionsInventory = collectionsInventory;
        }

        private void Awake()
        {
            rawImage = GetComponent<RawImage>();            
        }

        private void Start()
        {
            UpdateColor();
            collectionsInventory.OnCollectionChanged += UpdateColor;
        }

        private void Update()
        {
            rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(-xDirection * scrollSpeed, yDirection * scrollSpeed) * Time.deltaTime, rawImage.uvRect.size);
        }

        private void UpdateColor() => rawImage.color = collectionsInventory.SelectedCollection.BackgroundPatternColor;

        private void OnDestroy()
        {
            collectionsInventory.OnCollectionChanged -= UpdateColor;
        }
    }
}