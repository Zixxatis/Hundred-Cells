using UnityEngine;
using UnityEngine.UI;

namespace CGames
{
    public static class GraphicExtensions
    {
        /// <summary> Changes object's alpha to zero without changing RGB values. </summary>
        public static void MakeInvisible(this Graphic graphic)
        {
            graphic.color = new(graphic.color.r, graphic.color.g, graphic.color.b, 0);
        }

        /// <summary> Changes object's alpha to max. without changing RGB values. </summary>
        public static void MakeVisible(this Graphic graphic)
        {
            graphic.color = new(graphic.color.r, graphic.color.g, graphic.color.b, 1f);
        }

        /// <returns> Inverted color from given graphic, while preserving alpha. </returns>
        public static Color GetInvertedColor(this Graphic graphic)
        {
            return new
            (
                1f - graphic.color.r,
                1f - graphic.color.g,
                1f - graphic.color.b,
                graphic.color.a
            );
        }
    }
}