using System;

namespace WebUI.Data
{
    public static class DateExtension
    {
        /// <summary>
        /// Get a short string representation, like Q1, for the quarter of the year that this date falls in
        /// </summary>
        public static string Quarter(this DateTime? dt)
        {
            switch (dt?.Month)
            {
                case 1:
                case 2:
                case 3:
                    return "Q1";

                case 4:
                case 5:
                case 6:
                    return "Q2";

                case 7:
                case 8:
                case 9:
                    return "Q3";

                case 10:
                case 11:
                case 12:
                    return "Q4";
            }

            return null;
        }

        /// <summary>
        /// Get a short string representation, like Q1, for the quarter of the year that this date falls in
        /// </summary>
        public static string Quarter(this DateTime dt)
        {
            return Quarter((DateTime?)dt);
        }
    }
}
