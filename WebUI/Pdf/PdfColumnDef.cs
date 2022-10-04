using iTextSharp.text;
using System;
using System.Collections.Generic;

namespace WebUI.Pdf
{
    /// <summary>
    /// A model for the definition of one column in a PDF data table
    /// </summary>
    /// <typeparam name="T">the type of each row of data to be rendered</typeparam>
    public class PdfColumnDef<T>
    {
        /// <summary>
        /// default font for rendering text in this column
        /// </summary>
        public static readonly Font DEFAULT_FONT = new Font(Font.HELVETICA, 8);

        /// <summary>
        /// The function for getting a field value for this column out of a row of data
        /// </summary>
        public Func<T, object> GetFieldValue { get; set; }

        /// <summary>
        /// Optional function to explicitly transform the value for this column into a string for display
        /// </summary>
        public Func<T, string> GetDisplayValue { get; set; }

        /// <summary>
        /// The width of this column, as relative to all the other defined columns.
        /// This is not an absolute number, but is compared to the value given for all other defined columns
        /// in order to defermine how much of the table width to allot for each. Does not need to add up to 100.
        /// </summary>
        public float RelativeWidth { get; set; }

        /// <summary>
        /// Optional format string for rendering values in this column into text. 
        /// Any valid format string usable in String.Format should work. Leaving this null will still get you
        /// default formats for certain data types like dates and numerics.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// The font in which to render content in this column.
        /// </summary>
        public Font Font { get; set; } = DEFAULT_FONT;

        /// <summary>
        /// Horizontal alignment of values in this column
        /// </summary>
        public int HorizontalAlign { get; set; } = Element.ALIGN_RIGHT;

        /// <summary>
        /// Vertical alignment of values in this column
        /// </summary>
        public int VerticalAlign { get; set; } = Element.ALIGN_MIDDLE;

        /// <summary>
        /// Should this column's content be prevented from wrapping onto a new line if too long?
        /// </summary>
        public bool NoWrap { get; set; } = true;

        /// <summary>
        /// Should this column's title be prevented from wrapping onto a new line if too long?
        /// </summary>
        public bool NoWrapTitle { get; set; }

        /// <summary>
        /// Does this column contain money values?
        /// </summary>
        public bool IsMoney { get; set; } = false;

        /// <summary>
        /// Column header text
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Optional function for Aggregating (sum, average, etc) values in this column to display subtotals
        /// </summary>
        public Func<IEnumerable<T>, object> Aggregate { get; set; }

        /// <summary>
        /// Construct a new column definition with default values
        /// </summary>
        public PdfColumnDef() { }

        /// <summary>
        /// Construct a new column definition with the given values
        /// </summary>
        /// <param name="fieldGetter">Function for getting this column's values from data rows</param>
        /// <param name="title">The column header text</param>
        /// <param name="relativeWidth">The width of this column relative to others</param>
        /// <param name="isMoney">Does this column contain money values?</param>
        public PdfColumnDef(Func<T, object> fieldGetter, string title, float relativeWidth, bool isMoney = false)
        {
            GetFieldValue = fieldGetter;
            Title = title;
            RelativeWidth = relativeWidth;
            IsMoney = isMoney;
        }
    }
}
