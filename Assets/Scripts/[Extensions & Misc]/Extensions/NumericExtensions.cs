using System;

namespace CGames
{
    public static class NumericExtensions
    {
        /// <returns> True if the number is even. </returns>
        public static bool IsEven(this int value) => value % 2 == 0;

        /// <returns> True if the number is odd. </returns>
        public static bool IsOdd(this int value) => value % 2 == 1;

        /// <returns> True if the number is even. </returns>
        public static bool IsEven(this byte value) => value % 2 == 0;

        /// <returns> True if the number is odd. </returns>
        public static bool IsOdd(this byte value) => value % 2 == 1;


        /// <summary> Takes any INT and returns a postfix to it's value. </summary>
        /// <returns> A string with a number and two-lettered postfix for a given number </returns>
        public static string GetValueWithPostfix(this int value)
        {  
            // Returns exceptions for 11, 12, 13
            if(value == 11 || value % 100 == 11 || value == 12 || value % 100 == 12 || value == 13 || value % 100 == 13)
                return $"{value}th";

            return (value % 10) switch
            {
                1 => $"{value}st",
                2 => $"{value}nd",
                3 => $"{value}rd",
                _ => $"{value}th",
            };
        }

        /// <summary> Takes any byte and returns a postfix to it's value. </summary>
        /// <returns> A string with a number and two-lettered postfix for a given number </returns>
        public static string GetValueWithPostfix(this byte value)
        {   
            // Returns exceptions for 11, 12, 13
            if(value == 11 || value % 100 == 11 || value == 12 || value % 100 == 12 || value == 13 || value % 100 == 13)
                return $"{value}th";

            return (value % 10) switch
            {
                1 => $"{value}st",
                2 => $"{value}nd",
                3 => $"{value}rd",
                _ => $"{value}th",
            };
        }

        /// <summary> Takes any float and returns a postfix to it's value. </summary>
        /// <remarks> Will change float's value to it's FLOOR value. </remarks>
        /// <returns> A string with a floored number and two-lettered postfix for a given number. </returns>
        public static string GetValueWithPostfix(this float value)
        {   
            value = MathF.Floor(value);

            // Returns exceptions for 11, 12, 13
            if(value == 11 || value % 100 == 11 || value == 12 || value % 100 == 12 || value == 13 || value % 100 == 13)
                return $"{value}th";

            return (value % 10) switch
            {
                1 => $"{value}st",
                2 => $"{value}nd",
                3 => $"{value}rd",
                _ => $"{value}th",
            };
        }      
    }
}