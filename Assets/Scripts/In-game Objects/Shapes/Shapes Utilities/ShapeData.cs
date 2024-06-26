using System;

namespace CGames
{
    [Serializable]
    public class ShapeData : ICloneable<ShapeData>
    {
        public ShapeType ShapeType { get; private set; }
        public CellColor CellColor { get; private set; }
        public RotationDegrees RotationDegrees { get; private set; }
        
        public ShapeData(ShapeType shapeType, CellColor cellColor, RotationDegrees rotationDegrees)
        {
            ShapeType = shapeType;
            CellColor = cellColor;
            RotationDegrees = rotationDegrees;
        }

        public void ChangeColor(CellColor cellColor) => CellColor = cellColor;
        public void UpdateRotationValue(RotationDegrees rotationDegrees) => RotationDegrees = rotationDegrees;

        public ShapeData Clone()
        {
            return new ShapeData
            (
                ShapeType,
                CellColor,
                RotationDegrees
            );
        }
    }
}