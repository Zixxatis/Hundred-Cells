using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{   
    [RequireComponent(typeof(Graphic))]
    public abstract class ColorAdapter : MonoBehaviour
    {
        protected Graphic Graphic;
        protected CollectionsInventory CollectionsInventory;

        [Inject]
        private void Construct(CollectionsInventory collectionsInventory)
        {
            this.CollectionsInventory = collectionsInventory;
            Graphic = GetComponent<Graphic>();
        }

        protected virtual void Awake()
        {
            CollectionsInventory.OnCollectionChanged += UpdateColor;
        }

        protected virtual void Start()
        {
            UpdateColor();
        }

        protected abstract void UpdateColor();

        protected virtual void OnDestroy()
        {
            CollectionsInventory.OnCollectionChanged -= UpdateColor;
        }
    }
}