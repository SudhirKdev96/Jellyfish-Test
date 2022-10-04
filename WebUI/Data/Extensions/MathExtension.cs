using System;
using System.Collections.Generic;
using System.Linq;

namespace WebUI.Data
{
    public static class MathExtension
    {
        /// <summary>
        /// Find the standard deviation of a collection of values.
        /// Based on https://stackoverflow.com/a/6252351
        /// </summary>
        public static double StandardDeviation(this IEnumerable<float> values)
        {
            if (values == null) return 0;

            var avg = values.Average();
            return Math.Sqrt(values.Average(v => (v - avg) * (v - avg)));
        }
    }
}
