using System;

namespace CGames
{
    public static class RotationDegreesUtilities
    {
        public static RotationDegrees GetNextRotationDegrees(RotationDegrees previousDegrees)
        {
            return previousDegrees switch
            {
                RotationDegrees.None => RotationDegrees.Once,
                RotationDegrees.Once => RotationDegrees.Twice,
                RotationDegrees.Twice => RotationDegrees.Thrice,
                RotationDegrees.Thrice => RotationDegrees.None,
                _ => throw new NotSupportedException(),
            };
        }

        public static RotationDegrees GetPreviousRotationDegrees(RotationDegrees previousDegrees)
        {
            return previousDegrees switch
            {
                RotationDegrees.None => RotationDegrees.Thrice,
                RotationDegrees.Once => RotationDegrees.None,
                RotationDegrees.Twice => RotationDegrees.Once,
                RotationDegrees.Thrice => RotationDegrees.Twice,
                _ => throw new NotSupportedException(),
            };
        }
    }
}