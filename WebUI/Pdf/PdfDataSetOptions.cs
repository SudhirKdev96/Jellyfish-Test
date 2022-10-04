using System;
using System.Collections.Generic;
using System.Linq;

namespace WebUI.Pdf
{
    /// <summary>
    /// A model for options required to render a set of data into a PDF document
    /// </summary>
    /// <typeparam name="T">the type of each row of data to be rendered</typeparam>
    public class PdfDataSetOptions<T>
    {
        /// <summary>
        /// The full set of data to render into a PDF
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// The ordered definitions of how to group the given data
        /// </summary>
        public PdfGrouping<T>[] Groupings { get; set; }

        /// <summary>
        /// The ordered set of columns definitions for how to display the given data
        /// </summary>
        public PdfColumnDef<T>[] Columns { get; set; }

        /// <summary>
        /// The function for sorting table data in this document
        /// </summary>
        public Func<IEnumerable<T>, IOrderedEnumerable<T>> SortData { get; set; }

        /// <summary>
        /// How (if at all) should the builder update the relative column widths and 
        /// attempt to best fit the content of the data set to the available space?
        /// </summary>
        public PdfAutoSizeMethod AutoSizeMethod { get; set; } = PdfAutoSizeMethod.NONE;

        /// <summary>
        /// If this is not the first data set added to a PDF, should there be 
        /// space added between it and the previously-added data set?
        /// </summary>
        public bool AddSpaceBetweenSets { get; set; } = true;
    }

    public enum PdfAutoSizeMethod
    {
        /// <summary>
        /// Don't automatically adjust column widths
        /// </summary>
        NONE,

        /// <summary>
        /// Cut the relative width of any wrappable column in half
        /// </summary>
        AGGRESSIVE,

        /// <summary>
        /// Shrink particularly wide columns a bit at a time to find a good fit
        /// </summary>
        GENTLE
    }
}
