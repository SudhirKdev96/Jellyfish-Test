using iTextSharp.text;
using System;
using System.Collections.Generic;

namespace WebUI.Pdf
{
    /// <summary>
    /// A model for the definition of a column that displays a checkmark in a PDF data table
    /// </summary>
    /// <typeparam name="T">the type of each row of data to be rendered</typeparam>
    public class PdfCheckmarkColumnDef<T> : PdfColumnDef<T>
    {
        /// <summary>
        /// Construct a new column definition with the given values
        /// </summary>
        /// <param name="fieldGetter">Function for getting this column's values from data rows</param>
        /// <param name="title">The column header text</param>
        /// <param name="relativeWidth">The width of this column relative to others</param>
        public PdfCheckmarkColumnDef(Func<T, bool> fieldGetter, string title, float relativeWidth) 
            : base(fieldGetter as Func<T, object>, title, relativeWidth)
        {
            Font = new Font(Font.ZAPFDINGBATS, 8, Font.BOLD);
            GetDisplayValue = (x => fieldGetter(x) ? "\u0033" : "");
            GetFieldValue = GetDisplayValue;
            HorizontalAlign = Element.ALIGN_CENTER;
        }
    }
}
