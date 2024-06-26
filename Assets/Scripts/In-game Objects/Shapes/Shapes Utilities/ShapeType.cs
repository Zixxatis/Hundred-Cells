using UnityEngine;

namespace CGames
{
    public enum ShapeType
    {
        [InspectorName("Angle - Large")]
        AngleLarge,

        [InspectorName("Angle - Medium")]
        AngleMedium,
        
        [InspectorName("Line - Duo")]
        LineDuo,
        
        [InspectorName("Line - Trio")]
        LineTrio,

        [InspectorName("Plus")]
        Plus,
        
        [InspectorName("Square - Large")]
        SquareLarge,
        
        [InspectorName("Square - Medium")]
        SquareMedium,
        
        [InspectorName("Square - Small")]
        SquareSmall,
        
        [InspectorName("T-Shape - Large")]
        TLarge,

        [InspectorName("T-Shape - Medium")]
        TMedium,

        [InspectorName("U-Shape")]
        UShape,

        [InspectorName("Zig-zag")]
        Zigzag,

        [InspectorName("Zig-zag (Mirrored)")]
        ZigzagMirrored
    }
}