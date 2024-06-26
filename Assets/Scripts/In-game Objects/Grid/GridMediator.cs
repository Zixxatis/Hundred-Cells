using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    [RequireComponent(typeof(Image))]
    public class GridMediator : MonoBehaviour, IDropHandler, IShapePlacedNotifier, IGridMediatorActivator
    {
        public event Action OnShapePlaced;

        private GridValidator gridValidator;
        private BonusSystem bonusSystem;
        private ScoreCounter scoreCounter;
        private PlayerHand playerHand;
        private ISnapshotSaver snapshotSaver;
        
        private Action finishSessionAction;
        private Action<CoreSoundEffectType> playEffectAction;

        private Image raycastImage;

        [Inject]
        private void Construct(AudioPlayer audioPlayer, BonusSystem bonusSystem, ScoreCounter scoreCounter,
                               PlayerHand playerHand, GameStatusHandler gameStatusHandler, ISnapshotSaver snapshotSaver)
        {   
            this.bonusSystem = bonusSystem;
            this.scoreCounter = scoreCounter;
            this.playerHand = playerHand;
            this.snapshotSaver = snapshotSaver;
            this.finishSessionAction = gameStatusHandler.FinishSession;
            this.playEffectAction = (x) => audioPlayer.PlayCoreSFX(x);
        }

        public void Initialize(Cell[,] cellsMatrix)
        {
            gridValidator = new(cellsMatrix);
            snapshotSaver.Initialize(cellsMatrix);

            raycastImage = GetComponent<Image>();
            DisableGridMediatorRaycast();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if(eventData.pointerDrag.TryGetComponent(out Shape shape) == false)
                return;

            if (shape.TryToPlaceCellsOnGrid(out List<Cell> cells))
            {
                snapshotSaver.CaptureData();

                ShapeData cachedShapeData = new
                (
                    shape.ShapeData.ShapeType,
                    shape.ShapeData.CellColor,
                    shape.ShapeData.RotationDegrees
                );

                shape.ReleaseShape();
                playerHand.FillHandBySelectedGameMode();

                PlaceCells(cells, cachedShapeData.CellColor);
                HandleFilledLines();

                OnShapePlaced?.Invoke();

                ValidateGameStatus();

                DisableGridMediatorRaycast();
            }
        }

        private void PlaceCells(List<Cell> cells, CellColor cellColor)
        {
            foreach (Cell cell in cells)
            {
                cell.UpdateColor(cellColor);
            }

            scoreCounter.GivePointsForPlacingCell(cells.Count);
        }

        private void HandleFilledLines()
        {
            if (gridValidator.TryToFindFilledLines(out Dictionary<Cell, byte> filledCellsWithMultipliersDictionary))
            {
                IEnumerable<Cell> cellsInFilledLines = filledCellsWithMultipliersDictionary.Keys;
                
                bonusSystem.IncreaseCollectedCellsAmount(cellsInFilledLines);

                foreach (Cell cell in cellsInFilledLines)
                {
                    cell.ClearCell();
                }

                scoreCounter.AddPointsForClearedCells(filledCellsWithMultipliersDictionary.Values.ToList());

                CoreSoundEffectType coreSoundEffectType = filledCellsWithMultipliersDictionary.Any(x => x.Value >= 2)? CoreSoundEffectType.RemoveMultipleLines : CoreSoundEffectType.RemovedLine;
                playEffectAction.Invoke(coreSoundEffectType);
            }
            else
                playEffectAction.Invoke(CoreSoundEffectType.PlacedCell);
        }

        public void ValidateGameStatus()
        {
            if (gridValidator.CanPlaceAnyShapeFromHand(playerHand.ActualShapesInHand) == false)
                FinishSession();
        }

        public void EnableGridMediatorRaycast() => raycastImage.raycastTarget = true;
        public void DisableGridMediatorRaycast() => raycastImage.raycastTarget = false;
        
        [ContextMenu("Lose")]
        private void FinishSession()
        {
            snapshotSaver.CaptureData();
            finishSessionAction?.Invoke();

            playEffectAction.Invoke(CoreSoundEffectType.Lost);
        }
    }
}