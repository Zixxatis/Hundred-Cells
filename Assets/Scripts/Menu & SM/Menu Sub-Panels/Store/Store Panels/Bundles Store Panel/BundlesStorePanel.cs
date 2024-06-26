using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class BundlesStorePanel : StorePanel
    {
        [SerializeField] private Transform contentTransform;

        private StoreElementsFactory storeElementsFactory;
        private Func<BundleRarity, CoinsBundleSO> getCoinsBundle;

        private readonly List<StoreBundleSlot> storeBundleSlotsList = new();

        [Inject]
        private void Construct(StoreElementsFactory storeElementsFactory, ResourceSystem resourceSystem)
        {
            this.storeElementsFactory = storeElementsFactory;
            this.getCoinsBundle = resourceSystem.BundlesRSS.GetCoinsBundle;
        }

        private void Awake()
        {
            CreateBundleSlots();
        }

        private void CreateBundleSlots()
        {
            foreach(BundleRarity bundleRarity in Enum.GetValues(typeof(BundleRarity)))
            {
                StoreBundleSlot prefab = storeElementsFactory.GetBundleSlot(contentTransform);
                prefab.Initialize(getCoinsBundle(bundleRarity), ValidateValuesInSlots);

                storeBundleSlotsList.Add(prefab);
            }
        }

        protected override void PrepareBeforeOpening()
        {
            ValidateValuesInSlots();
        }

        private void ValidateValuesInSlots() => storeBundleSlotsList.ForEach(x => x.ValidateValues());

        protected override void PrepareBeforeClosing() { }
    }
}