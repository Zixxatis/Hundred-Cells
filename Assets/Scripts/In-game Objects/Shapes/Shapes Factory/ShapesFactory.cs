using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace CGames
{
    public class ShapesFactory
    {
        private readonly IInstantiator instantiator;
        private readonly ResourceSystem resourceSystem;
        private readonly Transform temporarySpawnPoint;

        private readonly Dictionary<ShapeType, ObjectPool<Shape>> shapesObjectPoolDictionary;

        public ShapesFactory(IInstantiator instantiator, ResourceSystem resourceSystem, DragCanvas dragCanvas)
        {
            this.instantiator = instantiator;
            this.resourceSystem = resourceSystem;
            this.temporarySpawnPoint = dragCanvas.DraggingZoneTransform;

            shapesObjectPoolDictionary = GetObjectPoolDictionary();
        }

        private Dictionary<ShapeType, ObjectPool<Shape>> GetObjectPoolDictionary()
        {
            IEnumerable<ShapeType> shapeTypes = Enum.GetValues(typeof(ShapeType)).Cast<ShapeType>();

            return shapeTypes.ToDictionary
            (
                x => x,
                x => new ObjectPool<Shape>
                (
                    () => CreateShape(x),
                    OnShapeGet,
                    OnShapeRelease,
                    OnShapeDestroy
                )
            );
        }

        private Shape CreateShape(ShapeType shapeType)
        {
            Shape shape = instantiator.InstantiatePrefabForComponent<Shape>(resourceSystem.ShapesRSS.GetShape(shapeType), temporarySpawnPoint);
            ObjectPool<Shape> shapeObjectPool = shapesObjectPoolDictionary[shapeType];

            shape.AssignReleaseAction(shapeObjectPool.Release);
            return shape;
        }

        private void OnShapeGet(Shape shape) => shape.ShapeDragHandler.EnableRaycast();
        private void OnShapeRelease(Shape shape) => shape.DeactivateGameObject();
        private void OnShapeDestroy(Shape shape) => UnityEngine.Object.Destroy(shape.gameObject);

        public Shape GetRandomShapeOfSize(ShapeSize shapeSize)
        {
            ShapeType shapeType = resourceSystem.ShapesRSS.GetRandomShapeTypeOfSize(shapeSize);
            return GetRandomShape(shapeType);
        }

        public Shape GetRandomShape(ShapeType shapeType)
        {
            ShapeData randomData = new
            (
                shapeType,
                Enum<CellColor>.GetRandomValueWithException((int)CellColor.None),
                Enum<RotationDegrees>.GetRandomValue()
            );

            return GetSpecificShape(randomData);
        }

        public Shape GetSpecificShape(ShapeData data)
        {
            Shape shape = shapesObjectPoolDictionary[data.ShapeType].Get();

            shape.ChangeRotationDegree(data.RotationDegrees);
            shape.SetColor(data.CellColor);

            return shape;
        }
    }
}