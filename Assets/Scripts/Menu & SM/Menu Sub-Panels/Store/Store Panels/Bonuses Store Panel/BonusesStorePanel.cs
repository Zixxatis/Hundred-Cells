using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class BonusesStorePanel : StorePanel
    {
        [SerializeField] private Transform contentTransform;

        private StoreElementsFactory storeElementsFactory;
        private readonly List<StoreBonusSlot> storeBonusSlotsList = new(Enum<BonusType>.Length);

        [Inject]
        private void Construct(StoreElementsFactory storeElementsFactory)
        {
            this.storeElementsFactory = storeElementsFactory;
        }

        private void Awake()
        {
            CreateBonusesSlots();
        }

        private void CreateBonusesSlots()
        {
            foreach(BonusType bonusType in Enum.GetValues(typeof(BonusType)))
            {
                StoreBonusSlot prefab = storeElementsFactory.GetBonusSlot(contentTransform);
                prefab.Initialize(bonusType, ValidateValuesInSlots);

                storeBonusSlotsList.Add(prefab);
            }
        }

        protected override void PrepareBeforeOpening()
        {
            ValidateValuesInSlots();
        }

        private void ValidateValuesInSlots() => storeBonusSlotsList.ForEach(x => x.ValidateValues());
        private void ResetValuesInSlots() => storeBonusSlotsList.ForEach(x => x.ResetValues());

        protected override void PrepareBeforeClosing()
        {
            ResetValuesInSlots();
        }
    }
}