using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class StoreVisualsSlot : MonoBehaviour
    {
        [Header("Colors Preview Elements")]
        [SerializeField] private List<ColorPreviewObject> colorPreviewObjectsList;
        [Space]
        [SerializeField] private Image backgroundImage;

        [Header("Info Elements")]
        [SerializeField] private TextLocalizer titleLTMP;
        [Space]
        [SerializeField] private GameObject priceObject;
        [SerializeField] private TextMeshProUGUI priceTMP;
        [SerializeField] private TextLocalizer statusLTMP;

        [Header("Buttons")]
        [SerializeField] private Button purchaseButton;
        [Space]
        [SerializeField] private Button selectButton;

        private CollectionsInventory collectionsInventory;
        private Wallet wallet;
        
        private CellColorsCollection cellColorsCollection;
        private Action validateValuesInAllSlotsAction;

        [Inject]
        private void Construct(CollectionsInventory collectionsInventory, Wallet wallet)
        {
            this.collectionsInventory = collectionsInventory;
            this.wallet = wallet;
        }

        public void Initialize(CellColorsCollection cellColorsCollection, Action validateValuesInAllSlotsAction)
        {
            this.cellColorsCollection = cellColorsCollection;
            this.validateValuesInAllSlotsAction = validateValuesInAllSlotsAction;

            SetVisuals();
            SetText();
            AddListenersToButtons();
            ValidateValues();
        }

        private void SetVisuals()
        {
            foreach(ColorPreviewObject colorPreviewObject in colorPreviewObjectsList)
            {
                colorPreviewObject.SetColor(cellColorsCollection.GetColor32ForCell(colorPreviewObject.CellColor));
            }

            backgroundImage.color = cellColorsCollection.BackgroundColor;
        }

        private void SetText()
        {
            titleLTMP.SetKeyAndUpdate(cellColorsCollection.TitleLK);
            priceTMP.text = cellColorsCollection.Price.ToString();
            statusLTMP.SetKeyAndUpdate("Status_Unlocked");
        }

        private void AddListenersToButtons()
        {
            purchaseButton.onClick.AddListener(Purchase);
            selectButton.onClick.AddListener(SelectCollection);
        }

        private void Purchase()
        {
            wallet.SpendCoins(cellColorsCollection.Price);
            collectionsInventory.UnlockCollection(cellColorsCollection.ColorsCollectionType);

            validateValuesInAllSlotsAction.Invoke();
        }

        private void SelectCollection()
        {
            collectionsInventory.ChangeCollection(cellColorsCollection.ColorsCollectionType);

            validateValuesInAllSlotsAction.Invoke();
        }

        public void ValidateValues()
        {
            if(collectionsInventory.IsCollectionUnlocked(cellColorsCollection.ColorsCollectionType))
                ShowForUnlocked();
            else
                ShowForLocked();
        }

        private void ShowForUnlocked()
        {
            priceObject.DeactivateObject();
            statusLTMP.ActivateGameObject();

            purchaseButton.DeactivateGameObject();
            selectButton.ActivateGameObject();

            if(collectionsInventory.IsCollectionSelected(cellColorsCollection.ColorsCollectionType))
            {
                selectButton.DeactivateGameObject();
                statusLTMP.SetKeyAndUpdate("Status_Selected");
            }
            else
            {
                selectButton.ActivateGameObject();
                statusLTMP.SetKeyAndUpdate("Status_Unlocked");
            }
        }

        private void ShowForLocked()
        {
            priceObject.ActivateObject();
            statusLTMP.DeactivateGameObject();

            purchaseButton.ActivateGameObject();
            selectButton.DeactivateGameObject();

            purchaseButton.ChangeInteractivityWithText(wallet.IsEnoughCoinsFor(cellColorsCollection.Price));
        }

        private void OnDestroy()
        {
            purchaseButton.onClick.RemoveListener(Purchase);
            selectButton.onClick.RemoveListener(SelectCollection);
        }

        #if UNITY_EDITOR
        [ContextMenu("Fill Preview List")]
        private void FillPreviewList()
        {   
            if(backgroundImage == null)
                return;

            colorPreviewObjectsList = new();

            foreach (Transform child in backgroundImage.transform)
            {
                if(child.TryGetComponent(out ColorPreviewObject colorPreviewObject))
                    colorPreviewObjectsList.Add(colorPreviewObject);
            }
            
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
        #endif
    }
}