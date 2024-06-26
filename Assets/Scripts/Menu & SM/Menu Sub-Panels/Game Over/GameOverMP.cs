using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

namespace CGames
{
    public class GameOverMP : MenuPanel
    {
        [Header("Game Over Panel - Elements")]
        [SerializeField] private TextMeshProUGUI scoreTMP;
        [SerializeField] private GameObject personalBestRecordObject;
        [SerializeField] private GameObject leaderboardRecordObject;
        [Space]
        [SerializeField] private Image gridBackgroundImage;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private GridAspectRatioMaintainer gridAspectRatioMaintainer;
        [Space]
        [SerializeField] private ShapeHolder firstShapeHolder;
        [SerializeField] private Image firstShapeHolderBackgroundImage;
        [SerializeField] private ShapeHolder secondShapeHolder;
        [SerializeField] private Image secondShapeHolderBackgroundImage;
        [SerializeField] private ShapeHolder thirdShapeHolder;
        [SerializeField] private Image thirdShapeHolderBackgroundImage;
        [Space]
        [SerializeField] private Button playAgainButton;

        private GridSnapshot gridSnapshot;
        private LeaderboardSystem leaderboardSystem;
        private SavingSystem savingSystem;
        private Func<ShapeData, Shape> getSpecificShape;
        private Action showAdvertisementPopupAction;
        private Func<Color32> getBackgroundColor;

        private readonly List<Cell> cellsList = new();
        private readonly List<ShapeHolder> shapeHolderList = new();

        [Inject]
        private void Construct(GridSnapshot gridSnapshot, LeaderboardSystem leaderboardSystem, SavingSystem savingSystem, ShapesFactory shapesFactory, 
                               AdvertisementCanvas advertisementCanvas, CollectionsInventory collectionsInventory) 
        {
            this.gridSnapshot = gridSnapshot;
            this.leaderboardSystem = leaderboardSystem;
            this.savingSystem = savingSystem;
            this.getSpecificShape = shapesFactory.GetSpecificShape;
            this.showAdvertisementPopupAction = advertisementCanvas.TryToOpenPopup;
            this.getBackgroundColor = () => collectionsInventory.SelectedCollection.BackgroundColor;
        }

        public void Initialize(Action restartSameModeAction)
        {
            playAgainButton.onClick.AddListener(restartSameModeAction.Invoke);
            playAgainButton.onClick.AddListener(ClosePanel);

            shapeHolderList.AddRange
            (
                new[] { firstShapeHolder, secondShapeHolder, thirdShapeHolder }
            );

            for (int i = 0; i < GameConstants.CellsAmount; i++)
            {
                cellsList.Add(gridLayoutGroup.transform.GetChild(i).GetComponent<Cell>());
            }
        }

        protected override void Awake()
        {
            base.Awake();

            personalBestRecordObject.DeactivateObject();
            leaderboardRecordObject.DeactivateObject();
        }

        private void OnEnable() => gridAspectRatioMaintainer.AdjustCellsSize();

        public override void PrepareBeforeOpening()
        {
            SessionData sessionData = gridSnapshot.GetDataFromSnapshot();

            ShowGrid(sessionData.CellsColorData);
            ShowShapes(sessionData.ShapesInHandDataList);
            scoreTMP.text = sessionData.Score.ToString();
            HandleLeaderboardChanges(sessionData.Score, sessionData.GameMode);

            SavePlayerAndSessionData();
        }

        private void ShowGrid(List<CellColor> cellsColorData)
        {
            for (int i = 0; i < cellsColorData.Count; i++)
            {
                cellsList[i].UpdateColor(cellsColorData[i]);
            }

            gridBackgroundImage.color = getBackgroundColor.Invoke();
        }

        private void ShowShapes(List<ShapeData> shapesInHandDataList)
        {
            for (int i = 0; i < shapesInHandDataList.Count; i++)
            {
                ShapeData shapeData = shapesInHandDataList[i];

                if(shapeData != null)
                {
                    Shape shape = getSpecificShape(shapeData);
                    shapeHolderList[i].BindShapeToHolder(shape, GameConstants.ShapeHolderReducedScale);
                }
            }

            firstShapeHolderBackgroundImage.color = getBackgroundColor.Invoke();
            secondShapeHolderBackgroundImage.color = getBackgroundColor.Invoke();
            thirdShapeHolderBackgroundImage.color = getBackgroundColor.Invoke();
        }

        private void HandleLeaderboardChanges(uint finalScore, GameMode gameMode)
        {
            if(leaderboardSystem.TryToUpdateLeaderboards(finalScore, gameMode, out LeaderboardUpdateType leaderboardUpdateType))
            {
                if(leaderboardUpdateType == LeaderboardUpdateType.PersonalBest || leaderboardUpdateType == LeaderboardUpdateType.Both)
                    personalBestRecordObject.ActivateObject();

                if(leaderboardUpdateType == LeaderboardUpdateType.LeaderboardRecord || leaderboardUpdateType == LeaderboardUpdateType.Both)
                    leaderboardRecordObject.ActivateObject();

                savingSystem.SaveData<RecordsData>();
            }     
        }

        private void SavePlayerAndSessionData()
        {
            savingSystem.SaveData<PlayerData>();
            savingSystem.SaveData<SessionData>();
        }

        public override void PrepareBeforeClosing() 
        {
            shapeHolderList.ForEach(x => x.UnbindShape());
            
            personalBestRecordObject.DeactivateObject();
            leaderboardRecordObject.DeactivateObject();
            
            showAdvertisementPopupAction.Invoke();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            playAgainButton.onClick.RemoveAllListeners();
        }
    }
}