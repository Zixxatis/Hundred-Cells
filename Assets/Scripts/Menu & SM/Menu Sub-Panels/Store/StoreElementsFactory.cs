using UnityEngine;
using Zenject;

namespace CGames
{
    public class StoreElementsFactory : MonoBehaviour
    {
        [Header("Bonuses")]
        [SerializeField] private StoreBonusSlot storeBonusSlotPrefab;

        [Header("Visuals")]
        [SerializeField] private StoreVisualsSlot storeVisualsSlotPrefab;

        [Header("Bundles")]
        [SerializeField] private StoreBundleSlot storeBundleSlotPrefab;

        private IInstantiator instantiator;

        [Inject]
        private void Construct(IInstantiator instantiator)
        {
            this.instantiator = instantiator;
        }

        public StoreBonusSlot GetBonusSlot(Transform spawnPoint)
        {
            return instantiator.InstantiatePrefabForComponent<StoreBonusSlot>(storeBonusSlotPrefab, spawnPoint);
        }

        public StoreVisualsSlot GetVisualsSlot(Transform spawnPoint)
        {
            return instantiator.InstantiatePrefabForComponent<StoreVisualsSlot>(storeVisualsSlotPrefab, spawnPoint);
        }

        public StoreBundleSlot GetBundleSlot(Transform spawnPoint)
        {
            return instantiator.InstantiatePrefabForComponent<StoreBundleSlot>(storeBundleSlotPrefab, spawnPoint);
        }
    }
}