using System;

namespace WebUI.Pdf
{
    /// <summary>
    /// A model for the definition of a way to group data in a PDF
    /// </summary>
    /// <typeparam name="T">the type of each row of data to be rendered</typeparam>
    public class PdfGrouping<T>
    {
        /// <summary>
        /// The function to get a field to be grouped on from row data
        /// </summary>
        public Func<T, string> ByField { get; set; }

        /// <summary>
        /// Optional label text to be prefixed on the headings for this grouping when rendered
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Should the members of this group be sorted alphabetically?
        /// </summary>
        public bool DoSortGroup { get; set; } = true;

        /// <summary>
        /// Should the values in this grouping be sorted in descending order?
        /// </summary>
        public bool SortDescending { get; set; }

        /// <summary>
        /// Construct a new grouping with the given values
        /// </summary>
        /// <param name="field">The function to get a field to be grouped on from row data</param>
        /// <param name="label">Optional label text to be prefixed on the headings for this grouping when rendered</param>
        /// <param name="sortDescending">Should the values in this grouping be sorted in descending order?</param>
        public PdfGrouping(Func<T, string> field, string label = null, bool sortDescending = false)
        {
            ByField = field;
            Label = label;
            SortDescending = sortDescending;
        }
    }
}
