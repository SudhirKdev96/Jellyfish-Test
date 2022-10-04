using System;
using System.Collections.Generic;
using System.Text;

namespace WebUI.Data.Models
{
    /// <summary>
    /// defines extension methods for Decimal
    /// </summary>
    public static class DecimalExtension
    {
        /// <summary>
        /// maps ones and tens int64 values to human-friendly strings (e.g. 6 -> six)
        /// </summary>
        public static Dictionary<long, string> onesAndTensMap = new Dictionary<long, string>()
        {
            { 0, "" },
            { 1, "One" },
            { 2, "Two" },
            { 3, "Three" },
            { 4, "Four" },
            { 5, "Five" },
            { 6, "Six" },
            { 7, "Seven" },
            { 8, "Eight" },
            { 9, "Nine" },
            { 10, "Ten" },
            { 11, "Eleven" },
            { 12, "Twelve" },
            { 13, "Thirteen" },
            { 14, "Fourteen" },
            { 15, "Fifteen" },
            { 16, "Sixteen" },
            { 17, "Seventeen" },
            { 18, "Eighteen" },
            { 19, "Nineteen" },
            { 20, "Twenty" },
            { 30, "Thirty" },
            { 40, "Forty" },
            { 50, "Fifty" },
            { 60, "Sixty" },
            { 70, "Seventy" },
            { 80, "Eighty" },
            { 90, "Ninety" }
        };

        /// <summary>
        /// maps powers of 1000 to human-friendly strings (e.g. 2 -> Million)
        /// </summary>
        public static Dictionary<long, string> powersMap = new Dictionary<long, string>()
        {
            { 0, "" },
            { 1, "Thousand" },
            { 2, "Million" },
            { 3, "Billion" },
            { 4, "Trillion" }
        };

        /// <summary>
        /// Converts a decimal to words for a check (e.g. 101.21 -> One Hundred One and 21/100)
        /// rounds to 2 decimal places (hundredths)
        /// </summary>
        /// <param name="number">the decimal to convert, range is ±999999999999999.99 (10^15 - 0.01)</param>
        /// <returns>a human-friendly string representation, e.g. "Two Thousand Twenty-One"</returns>
        public static string ToHumanFriendlyString(this decimal d)
        {
            StringBuilder result = new StringBuilder();

            // get the integral part of the decimal number
            long integer = Convert.ToInt64(Math.Floor(Math.Abs(d)));

            // handle negative numbers
            if (d < 0)
            {
                result.Append("Minus ");
            }

            // handle zero integral component and non-zero integral component
            if (integer == 0)
            {
                result.Append("Zero ");
            }
            else
            {
                // convert the integral component to words
                for (long exponent = 4; exponent >= 0; --exponent)
                {
                    // we want just the number of Math.Pow(10, exponent)s (thousands, millions, etc.)
                    long quantity = integer / Convert.ToInt64(Math.Pow(1000, exponent));
                    if (quantity > 0)
                    {
                        string hundreds = GetHundreds(quantity);
                        string tens = GetTens(quantity);
                        // build and append the quantity of this power of 1000 with appropriate spacing
                        if (hundreds.HasValue()) result.Append($"{hundreds} ");
                        if (tens.HasValue()) result.Append($"{tens} ");
                        // append the word for the power (e.g. "Million") if applicable with a space on the end
                        if ((hundreds.HasValue() || tens.HasValue()) && exponent > 0)
                        {
                            result.Append($"{powersMap[exponent]} "); 
                        }
                    }
                }
            }

            // get the hundredths (i.e. cents)
            long hundredths = Convert.ToInt32(Math.Round(((d - integer) * 100), 2));
            // convert the decimal component to words, make sure to 
            result.Append($"and {hundredths:00}/100");

            return result.ToString();
        }

        // get the human-friendly tens (and ones if applicable) from an int64 (e.g. 121 -> "Twenty-One")
        private static string GetTens(long value)
        {
            var sb = new StringBuilder();

            long number = value % 100;
            long ones = number % 10;
            long tens = number - ones;

            if (number < 20 || ones == 0)
            {
                // get directly from map
                sb.Append(onesAndTensMap[number]);
            }
            else
            {
                // build from map with hyphen
                sb.Append($"{onesAndTensMap[tens]}-{onesAndTensMap[ones]}");
            }

            return sb.ToString();
        }

        // get the human-friendly hundreds from an int64 (e.g. 4621 -> "Six Hundred")
        private static string GetHundreds(long value)
        {
            var sb = new StringBuilder();

            long hundreds = (value % 1000) / 100;
            if (hundreds > 0)
            {
                // build from map with space
                sb.Append($"{onesAndTensMap[hundreds]} Hundred");
            }

            return sb.ToString();
        }


        public static string ToCompactFormat(this decimal? d, string baseFormat)
        {
           if (d == null || d == 0) { return null; }

           if (Math.Abs(d ?? 0) < 1000) { return String.Format($"{{0:{baseFormat}}}", d); }

           if (Math.Abs(d ?? 0) < 1000000) { return String.Format($"{{0:{baseFormat}}}", d/1000) + "k"; }

           return String.Format($"{{0:{baseFormat}}}", d / 1000000) + "M";
        }
    }
}
