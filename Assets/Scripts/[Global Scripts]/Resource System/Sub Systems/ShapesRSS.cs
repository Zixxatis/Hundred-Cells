using System;
using System.Collections.Generic;
using System.Linq;

namespace CGames
{
    public class ShapesRSS
    {
        private readonly List<Shape> shapesList;
        private readonly Dictionary<ShapeType, ShapeSize> shapeSizeCache;

        private readonly DraggingDistanceConfig draggingDistanceConfig;

        public ShapesRSS(List<Shape> shapesList, DraggingDistanceConfig draggingDistanceConfig)
        {
            this.shapesList = shapesList;
            shapeSizeCache = this.shapesList.ToDictionary(x => x.ShapeType, x => x.ShapeSize);

            this.draggingDistanceConfig = draggingDistanceConfig;
        }

        public Shape GetShape(ShapeType shapeType) => shapesList.First(x => x.ShapeType == shapeType);

        public ShapeType GetRandomShapeTypeOfSize(ShapeSize shapeSize)
        {
            List<ShapeType> matchingShapeTypes = Enum.GetValues(typeof(ShapeType)).Cast<ShapeType>()
                                                     .Where(x => GetShapeSize(x) == shapeSize)
                                                     .ToList();

            if (matchingShapeTypes.Count == 0)
                throw new InvalidOperationException("No matching shape types found for the specified size.");
            else
                return matchingShapeTypes[UnityEngine.Random.Range(0, matchingShapeTypes.Count)];
        }

        private ShapeSize GetShapeSize(ShapeType shapeType)
        {
            if (shapeSizeCache.TryGetValue(shapeType, out var shapeSize))
                return shapeSize;
            else
                throw new KeyNotFoundException($"ShapeType {shapeType} not found in cache.");
        }

        public DraggingDistanceData GetDraggingDistanceData(DraggingDistance draggingDistance)
        {
            return draggingDistanceConfig.GetDraggingDistanceData(draggingDistance);
        }
    }
}