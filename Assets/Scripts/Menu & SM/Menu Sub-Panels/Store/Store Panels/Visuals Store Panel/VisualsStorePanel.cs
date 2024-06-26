using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class VisualsStorePanel : StorePanel
    {
        [SerializeField] private Transform contentTransform;

        private StoreElementsFactory storeElementsFactory;
        private Func<ColorsCollectionType, CellColorsCollection> getCellColorsCollection;

        private readonly List<StoreVisualsSlot> storeVisualsSlotsList = new(Enum<ColorsCollectionType>.Length);

        [Inject]
        private void Construct(StoreElementsFactory storeElementsFactory, ResourceSystem resourceSystem)
        {
            this.storeElementsFactory = storeElementsFactory;
            this.getCellColorsCollection = resourceSystem.ColorsRSS.GetCellColorsCollection;
        }

        private void Awake()
        {
            CreateVisualsSlots();
        }

        private void CreateVisualsSlots()
        {
            foreach(ColorsCollectionType collectionType in Enum.GetValues(typeof(ColorsCollectionType)))
            {
                StoreVisualsSlot prefab = storeElementsFactory.GetVisualsSlot(contentTransform);
                prefab.Initialize(getCellColorsCollection.Invoke(collectionType), ValidateValuesInSlots);

                storeVisualsSlotsList.Add(prefab);
            }
        }

        protected override void PrepareBeforeOpening()
        {            
            ValidateValuesInSlots();
        }

        private void ValidateValuesInSlots() => storeVisualsSlotsList.ForEach(x => x.ValidateValues());
        protected override void PrepareBeforeClosing() { }
    }
}