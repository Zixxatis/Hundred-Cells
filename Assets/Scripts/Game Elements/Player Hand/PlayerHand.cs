using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class PlayerHand : MonoBehaviour, ISavable<SessionData>
    {
        [SerializeField] private ShapeHolder firstShapeHolder;
        [SerializeField] private ShapeHolder secondShapeHolder;
        [SerializeField] private ShapeHolder thirdShapeHolder;
        private readonly List<ShapeHolder> shapeHoldersList = new();
        
        /// <returns> List of all shapes in Shape Holders, including empty. </returns>
        public IEnumerable<Shape> ShapesInHand => shapeHoldersList.Select(x => x.ShapeInSlot);

         /// <returns> List of all shapes in Shape Holders, excluding empty. </returns>
        public IEnumerable<Shape> ActualShapesInHand => shapeHoldersList.Where(x => x.ShapeInSlot != null).Select(x => x.ShapeInSlot);

        private GameStatusHandler gameStatusHandler;
        private IGameModeEnteredNotifier gameModeEnteredNotifier;
        private Func<ShapeSize, Shape> getRandomShape;
        private Func<ShapeData, Shape> getSpecificShape;

        [Inject]
        private void Construct(GameStatusHandler gameStatusHandler, ShapesFactory shapesFactory, IGameModeEnteredNotifier gameModeEnteredNotifier)
        {
            this.gameStatusHandler = gameStatusHandler;
            this.getRandomShape = shapesFactory.GetRandomShapeOfSize;
            this.getSpecificShape = shapesFactory.GetSpecificShape;
            this.gameModeEnteredNotifier = gameModeEnteredNotifier;

            gameStatusHandler.OnGameOver += ClearHandAndRespawnRandomly;
            gameModeEnteredNotifier.OnGameModeEntered += ReenableHand;

            shapeHoldersList.AddRange(new[]
            {
                firstShapeHolder,
                secondShapeHolder,
                thirdShapeHolder
            });
        }

        public void ReceiveData(SessionData data)
        {
            List<ShapeData> shapesInHandDataList = data.ShapesInHandDataList;

            if(shapesInHandDataList.IsNullOrEmpty())
                ClearHandAndRespawnRandomly();
            else
                ClearHandAndRespawnCertainShapes(shapesInHandDataList);
        }

        public void PassData(SessionData data)
        {
            List<ShapeData> shapeData = new();

            foreach (Shape shape in ShapesInHand)
            {
                if (shape == null)
                    shapeData.Add(null);
                else
                    shapeData.Add(shape.ShapeData);
            }

            data.ShapesInHandDataList = shapeData;
        }

        public void FillHandBySelectedGameMode()
        {
            switch (gameStatusHandler.GameMode)
            {
                case GameMode.Classic:
                    if(shapeHoldersList.TrueForAll(x => x.IsEmpty))
                    {
                        FillEmptySlotsWithRandomShapes();
                        break;
                    }
                    else
                        break;

                case GameMode.Infinite:
                    FillEmptySlotsWithRandomShapes();
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"{gameStatusHandler.GameMode} game mode has no implemented way of giving shapes.");
            }
        }

        private void SpawnShapesFromShapeData(List<ShapeData> shapesInHandDataList)
        {
            for (int i = 0; i < shapeHoldersList.Count; i++)
            {
                if(shapesInHandDataList[i] == null)
                    continue;

                SpawnSpecificShape(shapesInHandDataList[i], shapeHoldersList[i]);
            }
        }

        private void SpawnSpecificShape(ShapeData shapeData, ShapeHolder shapeHolder)
        {
            Shape shape = getSpecificShape.Invoke(shapeData);
            shapeHolder.BindShapeToHolder(shape);
        }
        
        private void FillEmptySlotsWithRandomShapes()
        {
            while (shapeHoldersList.Exists(x => x.IsEmpty))
            {
                SpawnRandomShapeToFirstEmptySlot();
            }
        }

        private void SpawnRandomShapeToFirstEmptySlot()
        {
            ShapeSize randomSize = ShapesRandomizer.GetRandomShapeSizeForCurrentHand(ShapesInHand, gameStatusHandler.GameMode);

            Shape shape = getRandomShape.Invoke(randomSize);
            ShapeHolder firstEmptyShapeHolder = shapeHoldersList.First(x => x.IsEmpty);

            firstEmptyShapeHolder.BindShapeToHolder(shape);
        }

        public void ClearHandAndRespawnRandomly()
        {
            ClearShapesFromHand();
            FillEmptySlotsWithRandomShapes();
        }

        public void ClearHandAndRespawnCertainShapes(List<ShapeData> shapesInHandDataList)
        {
            ClearShapesFromHand();
            SpawnShapesFromShapeData(shapesInHandDataList);
        }

        private void ClearShapesFromHand() => shapeHoldersList.ForEach(x => x.UnbindShape());

        private void ReenableHand()
        {
            foreach (Shape shape in ActualShapesInHand)
            {
                shape.DeactivateGameObject();
                shape.ActivateGameObject();
            }
        }
        
        private void OnDestroy()
        {
            gameStatusHandler.OnGameOver -= ClearHandAndRespawnRandomly;
            gameModeEnteredNotifier.OnGameModeEntered -= ReenableHand;
        }
    }
}