using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CGames
{
    public static class ShapesRandomizer
    {
        public static List<ShapeType> GetThreeUniqueShapeTypes()
        {
            ShapeType firstShapeType = Enum<ShapeType>.GetRandomValue();
            ShapeType secondShapeType = Enum<ShapeType>.GetRandomValueWithException((int)firstShapeType);
            ShapeType thirdShapeType = Enum<ShapeType>.GetRandomValueWithException(new List<int>{ (int)firstShapeType, (int)secondShapeType });

            return new()
            {
                firstShapeType, secondShapeType, thirdShapeType
            };
        }

        public static ShapeSize GetRandomShapeSizeForCurrentHand(IEnumerable<Shape> shapesInHandList, GameMode currentGameMode)
        {
            bool shouldGiveNonLargeShapeSize = currentGameMode == GameMode.Classic && 
                                               shapesInHandList.Count(x => x != null && x.ShapeSize == ShapeSize.Large) == shapesInHandList.Count() - 1;

            if(shouldGiveNonLargeShapeSize)
                return GetRandomShapeSizeExcludingLarge();
            else
                return GetRandomShapeSize();
        }

        private static ShapeSize GetRandomShapeSize()
        {
            const int ChanceForSmallShape = 25;
            const int ChangeForMediumShape = 55;
            // ? const int ChangeForLargeShape = 20;

            return Random.Range(1, 101) switch
            {
                <= ChanceForSmallShape => ShapeSize.Small,
                <= ChanceForSmallShape + ChangeForMediumShape => ShapeSize.Medium,
                _ => ShapeSize.Large
            };
        }
        
        private static ShapeSize GetRandomShapeSizeExcludingLarge()
        {
            const int ChanceForSmallShape = 60;
            // ? const int ChangeForMediumShape = 40;
            // ? const int ChangeForLargeShape = 0;

            return Random.Range(1, 101) switch
            {
                <= ChanceForSmallShape => ShapeSize.Small,
                _ => ShapeSize.Medium
            };
        } 
    }
}