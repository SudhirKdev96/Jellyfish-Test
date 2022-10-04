using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WebUI.Data;
using WebUI.Data.Models;

namespace WebUI.Pdf
{
    public class PdfBuilder : IDisposable
    {
        // flag set the first time Dispose() is called
        private bool _alreadyDisposed;

        // stream for holding on to the document as it's built
        private readonly MemoryStream _memoryStream;

        // the final result, set once the document is closed
        private byte[] _finalBytes;

        // a running count of the number of separate data sets added to the document
        private int _dataSetsAdded;

        protected Document Document { get; set; }

        /// <summary>
        /// Create a new PDF wrapper for the given document, and associate it with a memory stream
        /// </summary>
        /// <param name="docOptions">options to override defaults related to the overall PDF document</param>
        public PdfBuilder(PdfDocumentOptions docOptions)
        {
            if (docOptions == null)
            {
                throw new ArgumentNullException(nameof(docOptions), "Document options are missing");
            }

            // prepare the page layout (size and rotation)
            var paper = docOptions.UseLandscapeOrientation
                ? docOptions.PaperSize.Rotate()
                : docOptions.PaperSize;

            // the top-level document object we're building
            Document = new Document(paper);

            // associate document with a memory stream for building the document in memory
            _memoryStream = new MemoryStream();
            PdfWriter.GetInstance(Document, _memoryStream);

            // configure the header and footer before opening the document
            BuildPdfHeaderFooter(docOptions);

            // open the document to start adding body content
            Open();
        }

        /// <summary>
        /// Opens the PDF document for writing pages 
        /// (header and footer must already have been set before this is called)
        /// </summary>
        protected void Open()
        {
            if (!Document.IsOpen())
            {
                Document.Open();
            }
        }

        /// <summary>
        /// Closes the document and writes out the memory stream to bytes.
        /// No further data can be added after this call. Idempotent.
        /// </summary>
        /// 
        /// <returns>a byte array of the PDF data suitable for download or display</returns>
        protected void Close()
        {
            if (Document.IsOpen())
            {
                Document.Close();

                _finalBytes = _memoryStream.ToArray();

                // I got no more use for dis guy
                _memoryStream.Dispose();
            }
        }

        /// <summary>
        /// Get the final PDF build as raw bytes.
        /// Closes the document if it's still open.
        /// No further data can be added after this call.
        /// </summary>
        public byte[] GetBytes()
        {
            Close();
            return _finalBytes;
        }

        /// <summary>
        /// Get the final PDF build as a base-64 string ready to save to file.
        /// Closes the document if it's still open.
        /// No further data can be added after this call.
        /// </summary>
        public string GetBase64String()
        {
            return Convert.ToBase64String(GetBytes());
        }

        /// <summary>
        /// validate the given options
        /// </summary>
        /// 
        /// <returns>whether or not there is any data provided in the options</returns>
        private static bool ValidateOptions<T>(PdfDataSetOptions<T> opts)
        {
            if (opts.Columns == null || !opts.Columns.Any())
            {
                throw new ArgumentException("Columns missing from options");
            }

            if (opts.Data == null || !opts.Data.Any())
            {
                Debug.WriteLine("Options with no data provided");
                return false;
            }

            return true;
        }


        /// <summary>
        /// configures the header and footer sections for a document
        /// </summary>
        private void BuildPdfHeaderFooter(PdfDocumentOptions docOptions)
        {
            #region Header
            if (docOptions.Title != null)
            {
                var title = new Chunk(docOptions.Title, docOptions.HeaderFont);
                var header = new HeaderFooter(new Phrase(title), false)
                {
                    Alignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER
                };
                Document.Header = header;
            }
            #endregion

            #region Footer
            // as far as I can tell, there's not really a good way to space out parts of the footer,
            // so this is a kludge that inserts a bunch of spaces where we need them
            var spacer = new Chunk(" ");
            // company name on the left side
            var leftPhrase = new Phrase(new Chunk(docOptions.FooterText, docOptions.FooterFont));
            // add a bunch of space
            for (int i = 0; i < 48; i++)
            {
                leftPhrase.Add(spacer);
            }
            // page number will automatically get added between the left and right phrases, so prefix it with text here
            leftPhrase.Add(new Chunk("Page ", docOptions.FooterFont));

            // the current date is going on the right, but needs a bunch of space after the page number
            var rightPhrase = new Phrase();
            for (int i = 0; i < 62; i++)
            {
                rightPhrase.Add(spacer);
            }
            // format the date and append
            rightPhrase.Add(new Chunk(String.Format("{0:d}", DateTime.Today), docOptions.FooterFont));
            var footer = new HeaderFooter(leftPhrase, rightPhrase)
            {
                Border = Rectangle.TOP_BORDER,
                Alignment = Element.ALIGN_CENTER,

            };
            Document.Footer = footer;
            #endregion
        }

        /// <summary>
        /// Render and append a set of data into this PDF
        /// </summary>
        /// <typeparam name="T">the type of data being exported</typeparam>
        /// <param name="pdf">A PDF document in progress, as created by StartPdf</param>
        /// <param name="dataOptions">the data to be added and options for turning that data into PDF items</param>
        /// <param name="addReportTotals">add report totals at the end based on the current data set?</param>
        /// <returns>the same pdf object for chaining</returns>
        public PdfBuilder AddDataSet<T>(PdfDataSetOptions<T> dataOptions, bool addReportTotals = false)
        {
            try
            {
                // only run if there's data
                if (ValidateOptions(dataOptions))
                {
                    if (dataOptions.AutoSizeMethod != PdfAutoSizeMethod.NONE)
                    {
                        AutoSizeColumns(dataOptions.Columns, dataOptions.Data, dataOptions.AutoSizeMethod);
                    }

                    BuildPdfBody(dataOptions, addReportTotals);
                    _dataSetsAdded++;
                }
                return this;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"PdfBuilder.AddDataSet: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Looks at the actual data in each column and tries to set the column widths relative to
        /// each other in a way that best fits the available space
        /// </summary>
        private static void AutoSizeColumns<T>(PdfColumnDef<T>[] columnDefs, IEnumerable<T> data, PdfAutoSizeMethod method)
        {
            if (columnDefs == null || columnDefs.Length < 1) return;

            foreach (var colDef in columnDefs)
            {
                // get the column header length (or just the longest word), and pad it a little bit since it's bold
                float lengthOfTitle = 2 + (colDef.NoWrapTitle 
                    ? (colDef.Title?.Length ?? 0)
                    : colDef.Title.LengthOfLongestWord());

                // get the longest value in the column when converted to string
                var longestValue = data.Select(x => colDef.GetFieldValue(x))
                    .Select(v => new { Value = v, Length = v.Format(colDef.IsMoney).Length })
                    .OrderByDescending(x => x.Length)
                    .First();

                // get the length of the formatted aggregate value, which could be longer than any individual value in the column
                int lengthOfAggregateValue = (colDef.Aggregate == null) ? 0 
                    : colDef.Aggregate(data).Format(colDef.IsMoney).Length;

                // pick the longer of the two
                float lengthOfLongestValue = Math.Max(longestValue.Length, lengthOfAggregateValue);

                // numeric columns are likely to contain a high percentage of zeroes, which are wider
                // since we're not working with a fixed-width font, so pad this length a little
                if (longestValue.Value.IsNumeric()) lengthOfLongestValue += 2;

                // With the aggressive method, if this column can wrap, just
                // cut its relative width in half to account for 2 lines of text
                if (PdfAutoSizeMethod.AGGRESSIVE == method && !colDef.NoWrap)
                {
                    lengthOfLongestValue /= 2f;
                }

                // get the bigger of the longest header word or the longest data value
                float maxLength = Math.Max(lengthOfTitle, lengthOfLongestValue);

                // set the column relative width
                colDef.RelativeWidth = maxLength;
                //Debug.WriteLine($"PdfBuilder.AutoSizeColumns: Set {colDef.Title} rel width to {maxLength} (longest word {lengthOfLongestValue})");
            }

            // With the gentle method, adjust wider columns a little bit at a time
            if (PdfAutoSizeMethod.GENTLE == method)
            {
                AdjustColumnsIncrementally(columnDefs);
            }
        }

        /// <summary>
        /// Do some statistical math to iteratively shrink wider columns down
        /// </summary>
        private static void AdjustColumnsIncrementally<T>(PdfColumnDef<T>[] columnDefs)
        {
            // figure out the average column [relative] width and the standard deviation
            var widths = columnDefs.Select(x => x.RelativeWidth);
            var avgLen = widths.Average();
            var stdDev = widths.StandardDeviation();
            if (stdDev < 1) return;

            bool didAdjustAny = false;
            foreach (var colDef in columnDefs)
            {
                // if this multi-line column is sized well above the average, shrink it a bit
                var width = colDef.RelativeWidth;
                var deviation = (width - avgLen) / stdDev;
                if (!colDef.NoWrap && deviation > 1.1)
                {
                    colDef.RelativeWidth = width * 0.9f;
                    didAdjustAny = true;
                }
            }

            // if any columns were adjusted above, iterate until they're all under the limit
            if (didAdjustAny)
            {
                AdjustColumnsIncrementally(columnDefs);
            }
        }

        /// <summary>
        /// Manually add a single row of totals to the end of this document
        /// </summary>
        /// 
        /// <param name="totalsData">a single row of data</param>
        /// 
        /// <param name="columnDefs">the definition of the columns used to render the totals data.
        /// Relevant bits are the column relative widths and whether each column is set to show an aggregate.
        /// Recommended that this be the same column definitions as the last data set added
        /// so that the columns line up.</param>
        public void AddReportTotals<T>(T totalsData, PdfColumnDef<T>[] columnDefs)
        {
            var bodyTable = PrepTable(columnDefs);
            AddTotalsRow(bodyTable, columnDefs, new T[] { totalsData }, "Report");
            Document.Add(bodyTable);
        }

        /// <summary>
        /// Adds a Paragraph with a simple string to the PDF
        /// Use to add footnotes, etc.
        /// </summary>
        /// <param name="text">The text to append to the report</param>
        /// <param name="alignment">optionally specify the text alignment on the page. Use iTextsharp.text.Element constants</param>
        /// <param name="indent">optionally specify left-side indentation for all lines in the given text</param>
        public PdfBuilder AddText(string text, int alignment = Element.ALIGN_LEFT, float indent = 0)
        {
            var table = new PdfPTable(1);
            var cell = new PdfPCell();
            var font = new Font(Font.HELVETICA, 8);
            Paragraph p = new Paragraph(text, font)
            {
                Alignment = alignment,
                IndentationLeft = indent
            };
            cell.AddElement(p);
            table.AddCell(cell);
            Document.Add(p);

            return this;
        }

        /// <summary>
        /// Add a standalone section heading in the same style as the group headings that are created with data sets
        /// </summary>
        /// <param name="text">text to display in the heading</param>
        /// <param name="level">the dataset group level whose style to mimic</param>
        /// <param name="alignment">horizontal alignment for the text</param>
        /// <param name="emptyRowsToAddAbove">how many empty rows to add as space above this heading. Defaults to none</param>
        /// <returns></returns>
        public PdfBuilder AddSectionHeading(string text, int level, int alignment, int emptyRowsToAddAbove = 0)
        {
            var table = new PdfPTable(1) { WidthPercentage = 100 };
            AddBlankRows(table, emptyRowsToAddAbove);
            AddPdfGroupHeading(bodyTable: table, groupLevel: level, text: text, hAlign: alignment);
            Document.Add(table);

            return this;
        }

        /// <summary>
        /// Prepare basic properties of a new table to be added into the document.
        /// </summary>
        /// <param name="colDefs">the definitions of each column in this table</param>
        /// <returns>a new table with column count and relative widths set</returns>
        private static PdfPTable PrepTable<T>(PdfColumnDef<T>[] colDefs)
        {
            // pull the relative column widths out of the definitions
            float[] relativeColWidths = colDefs.Select(c => c.RelativeWidth).ToArray();

            // initialize the table that will hold all body contents
            var bodyTable = new PdfPTable(relativeColWidths)
            {
                // usurp all horizontal space
                WidthPercentage = 100
            };

            return bodyTable;
        }

        /// <summary>
        /// Add any number of blank rows onto the given table
        /// </summary>
        private static void AddBlankRows(PdfPTable bodyTable, int numRowsToAdd)
        {
            var columnCount = bodyTable?.NumberOfColumns ?? 0;

            if (columnCount < 1 || numRowsToAdd < 1) return;

            for (int i=0; i<columnCount; i++)
            {
                bodyTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER });
            }

            bodyTable.CompleteRow();

            AddBlankRows(bodyTable, numRowsToAdd - 1);
        }

        /// <summary>
        /// build out the body contents
        /// </summary>
        private void BuildPdfBody<T>(PdfDataSetOptions<T> opts, bool addReportTotals)
        {
            // add some blank space between data sets
            if (_dataSetsAdded > 0 && opts.AddSpaceBetweenSets)
            {
                // add it as a new table so as to not mess with column headers repeating
                var t = new PdfPTable(1);
                AddBlankRows(t, 2);
                Document.Add(t);
            }

            // the full set of report data
            var data = opts.Data;

            // the definition of columns to be tabularized
            var colDefs = opts.Columns;

            var bodyTable = PrepTable(colDefs);

            #region Column Headers
            // define the table header row that repeats on every page
            if (colDefs.Any(x => x.Title.HasValue()))
            {
                bodyTable.HeaderRows = 1;
                foreach (var colDef in colDefs)
                {
                    var cell = BuildCell(colDef.Title, colDef, true, true);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = Rectangle.BOTTOM_BORDER;
                    cell.NoWrap = colDef.NoWrapTitle;
                    bodyTable.AddCell(cell);
                }
                bodyTable.CompleteRow();
            }
            #endregion

            // start iterating through data groupings to add to the table
            AddNextPdfGroup(bodyTable, opts, colDefs, data);

            if (addReportTotals)
            {
                // add a final totals row for the whole report
                AddTotalsRow(bodyTable, colDefs, data, "Report");
            }

            // write the whole shebang to the document in memory
            Document.Add(bodyTable);
        }

        /// <summary>
        /// An iterable addition of a data grouping to the table being constructed
        /// </summary>
        /// <param name="bodyTable">the table being constructed</param>
        /// <param name="opts">the overall options for PDF generation</param>
        /// <param name="colDefs">the definitions of each column in this table</param>
        /// <param name="dataForThisGroup">the data to render under this grouping</param>
        /// <param name="groupLevel">an indicator of how "high" this grouping is relative to others,
        /// where lower numbers are higher-level, more-prominent groupings, starting at 0</param>
        private static void AddNextPdfGroup<T>(PdfPTable bodyTable, PdfDataSetOptions<T> opts,
            PdfColumnDef<T>[] colDefs, IEnumerable<T> dataForThisGroup, int groupLevel = 0)
        {
            // get all the groupings configured in options
            var groupings = opts.Groupings;

            // if there are no groupings defined, or if we've drilled down to the last grouping,
            // sort and render the data for this grouping into the table
            if (groupings == null || groupLevel >= groupings.Length)
            {
                var sortedData = opts.SortData == null ? dataForThisGroup : opts.SortData(dataForThisGroup);
                AddDataToTable(bodyTable, sortedData, colDefs);
                return;
            }

            // grab the grouping definition for this level
            var grouping = groupings[groupLevel];

            // skip a level if it's set to null
            if (grouping == null)
            {
                AddNextPdfGroup(bodyTable, opts, colDefs, dataForThisGroup, groupLevel + 1);
                return;
            }

            // pull out the distinct members of this grouping
            var groupMembers = dataForThisGroup.Select(d => grouping.ByField(d)).Distinct().ToList();

            if (grouping.DoSortGroup)
            {
                // sort the grouping members (then reverse if descending)
                groupMembers.Sort(StringComparer.CurrentCultureIgnoreCase);
                if (grouping.SortDescending)
                {
                    groupMembers.Reverse();
                }
            }

            // iterate through each member of this grouping
            foreach (var groupMember in groupMembers)
            {
                // render the heading text for this member
                AddPdfGroupHeading(bodyTable, groupLevel, groupMember, grouping.Label);

                // pull out the data applicable only to this group member
                var dataForNextGroup = dataForThisGroup.Where(d => grouping.ByField(d) == groupMember);

                // drill down to the next grouping level
                AddNextPdfGroup(bodyTable, opts, colDefs, dataForNextGroup, groupLevel + 1);

                // add a totals row if the data for this group has more than one row, and the group is labeled
                if (dataForNextGroup.Count() > 1 && groupMember.HasValue())
                {
                    // add space before the totals row for separation unless this is the last grouping
                    var addSpaceBefore = groupLevel + 1 < groupings.Length;
                    AddTotalsRow(bodyTable, colDefs, dataForNextGroup, groupMember, addSpaceBefore);
                }
            }
        }

        /// <summary>
        /// Render heading text for a group member, with formatting dependent on the group level
        /// </summary>
        /// <param name="bodyTable">the table being constructed</param>
        /// <param name="groupLevel">an indicator of how "high" this grouping is relative to others,
        /// where lower numbers are higher-level, more-prominent groupings, starting at 0</param>
        /// <param name="text">The text description of this group member</param>
        /// <param name="label">An optional label to prefix the member description with</param>
        /// <param name="hAlign">optional horizontal alignment to override the default alignment</param>
        private static void AddPdfGroupHeading(PdfPTable bodyTable, int groupLevel, string text, 
            string label = null, int? hAlign = null)
        {
            switch (groupLevel)
            {
                case 0:
                    // Top level, most prominent grouping
                    // add a blank row before it for spacing
                    var blankRow = new PdfPCell(new Phrase())
                    {
                        Colspan = bodyTable.NumberOfColumns,
                        Border = Rectangle.NO_BORDER,
                        FixedHeight = 10
                    };
                    bodyTable.AddCell(blankRow);

                    if (text.HasValue())
                    {
                        // then a row for the biggie sized heading
                        var font = new Font(Font.TIMES_ROMAN, 14, Font.BOLD, BaseColor.Black);
                        // render heading text into a big box with vertical padding
                        var headingRow = new PdfPCell(BuildHeading(font, text, label))
                        {
                            Colspan = bodyTable.NumberOfColumns,
                            Border = Rectangle.BOX,
                            HorizontalAlignment = hAlign ?? Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            UseAscender = true,
                            PaddingTop = 5,
                            PaddingBottom = 5
                        };
                        bodyTable.AddCell(headingRow);
                    }
                    break;

                case 1:
                case 2:
                    if (text.HasValue())
                    {
                        var font = new Font(Font.TIMES_ROMAN, 12, Font.BOLD, BaseColor.Black);
                        var headingRow = new PdfPCell(BuildHeading(font, text, label))
                        {
                            Colspan = bodyTable.NumberOfColumns,
                            Border = 0,
                            HorizontalAlignment = hAlign ?? Element.ALIGN_LEFT
                        };
                        bodyTable.AddCell(headingRow);
                    }
                    break;

                default:
                    // if future reports call for more than 2 levels of headers, add additional case statements
                    var msg = $"Unhandled group level ({groupLevel}) passed to AddPdfGroupHeading";
                    Debug.WriteLine(msg);
                    throw new ArgumentOutOfRangeException(msg);
            }
        }

        /// <summary>
        /// Build a heading phrase from the given font, text, and optional label
        /// </summary>
        /// <param name="font">The font to show text in</param>
        /// <param name="text">The text value that to always be rendered</param>
        /// <param name="label">option prefix to be inserted before the given text</param>
        private static Phrase BuildHeading(Font font, string text, string label = null)
        {
            var phrase = new Phrase();
            if (!String.IsNullOrWhiteSpace(label))
            {
                phrase.Add(new Chunk($"{label}: ", font));
            }
            phrase.Add(new Chunk(text, font));

            return phrase;
        }

        /// <summary>
        /// Add a row of (sub)totals for the given data based on column definitions
        /// </summary>
        /// <param name="bodyTable">the table being constructed</param>
        /// <param name="colDefs">the definitions of each column in this table</param>
        /// <param name="dataToAggregate">the set of data to be totaled up</param>
        /// <param name="groupLabel">a text description of the grouping this data applies to</param>
        /// <param name="addSpaceBefore">add a row of empty space before appending the totals row?</param>
        private static void AddTotalsRow<T>(PdfPTable bodyTable, PdfColumnDef<T>[] colDefs,
            IEnumerable<T> dataToAggregate, string groupLabel, bool addSpaceBefore = true)
        {
            // Prep an array of cells to span a full row of this table
            var numCols = colDefs.Length;
            var totalRowCells = new PdfPCell[numCols];

            // keep track of the first column that is configured for aggregation
            int firstColWithAggregate = -1;

            // step through each column
            for (int i = 0; i < numCols; i++)
            {
                var colDef = colDefs[i];

                // if the column isn't defined with an aggregate function then move on
                if (colDef.Aggregate == null) continue;

                // aggregate the data in this column
                var aggregateVal = colDef.Aggregate(dataToAggregate);

                // create a new cell with the aggregate value
                totalRowCells[i] = BuildCell(aggregateVal, colDef, true);

                if (firstColWithAggregate < 0)
                {
                    firstColWithAggregate = i;
                }
            }

            // Add a total row if any columns are aggregated
            if (firstColWithAggregate >= 0)
            {
                if (addSpaceBefore) AddBlankRows(bodyTable, 1);

                // Create a label if there is room in the first column
                if (firstColWithAggregate > 0)
                {
                    // Label based on the name of this group
                    var label = $"{groupLabel} Totals: ";
                    // Grab the font from the first total cell
                    Font font = totalRowCells[firstColWithAggregate].Phrase.Font;
                    // make sure the label is black even if the total cell isn't (e.g. red when the value is negative)
                    Font labelFont = new Font(font) { Color = BaseColor.Black };
                    // Build the cell and span it across all the empty cells from the left
                    var labelCell = BuildCell(label, labelFont, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                    labelCell.Colspan = firstColWithAggregate;
                    labelCell.Border = Rectangle.NO_BORDER;
                    // Put it in the array of cells for this total row
                    totalRowCells[0] = labelCell;
                }

                // Add the cells to the table
                for (int i = 0; i < totalRowCells.Length;)
                {
                    // Use a blank cell to "skip" columns with no aggregate
                    var cell = totalRowCells[i] ?? new PdfPCell(new Phrase()) { Border = Rectangle.NO_BORDER };

                    bodyTable.AddCell(cell);
                    // skip ahead based on colspan
                    i += cell.Colspan;
                }

                // Done with the totals row
                bodyTable.CompleteRow();
            }
        }

        /// <summary>
        /// Build a single data cell, formatted based on a column definition
        /// </summary>
        /// <param name="value">the value to be rendered into a cell</param>
        /// <param name="colDef">the definition of the column this cell goes in</param>
        /// <param name="makeBold">optional override to indicate if this cell's text should be bolded</param>
        /// <param name="useDefaultFont">if true, ignore the font specified in the column definition and use the default</param>
        /// <returns>a new cell ready to add to a PdfPTable</returns>
        private static PdfPCell BuildCell<T>(object value, PdfColumnDef<T> colDef, bool makeBold = false, bool useDefaultFont = false)
        {
            // use either a specific format, if provided, or a default format for the value's Type
            string format = colDef.Format ?? value?.GetType()?.GetDefaultFormat(colDef.IsMoney);

            // negative money values should be red
            bool makeRed = colDef.IsMoney && ((value as decimal?) ?? 0) < 0;

            // prepare the cell font
            var font = new Font(useDefaultFont ? PdfColumnDef<T>.DEFAULT_FONT : colDef.Font);
            if (makeRed)
            {
                font.Color = BaseColor.Red;
            }
            if (makeBold)
            {
                font.SetStyle(Font.BOLD);
            }

            // build the cell
            return BuildCell(value, font, colDef.HorizontalAlign, colDef.VerticalAlign, format, colDef.NoWrap);
        }

        /// <summary>
        /// Build a single data cell with the provided format options
        /// </summary>
        /// <param name="value">the value to be rendered into a cell</param>
        /// <param name="font">the font to render text in this cell</param>
        /// <param name="hAlign">horizontal alignment of the cell's contents</param>
        /// <param name="vAlign">vertical alignment of the cell's contents</param>
        /// <param name="format">optional format string for displaying the given value as text</param>
        /// <param name="noWrap">optional override to indicate if text in this cell is allowed to wrap to a new line</param>
        /// <returns>a new cell ready to add to a PdfPTable</returns>
        private static PdfPCell BuildCell(object value, Font font, int hAlign, int vAlign, string format = null, bool noWrap = true)
        {
            // get the appropriate string representation of the given value
            string text = (format == null) ? value?.ToString()
                : string.Format($"{{0:{format}}}", value);

            // put the text in a new cell with the provided options
            var phrase = new Phrase(text, font);
            var cell = new PdfPCell(phrase) 
            { 
                HorizontalAlignment = hAlign,
                NoWrap = noWrap,
                // vertically center and pad the data with a little whitespace
                VerticalAlignment = vAlign,
                UseAscender = true,
                Padding = 3
            };

            return cell;
        }

        /// <summary>
        /// Tabularizes a set of data by adding rows onto an existing table
        /// </summary>
        /// <param name="t">the table to be appended</param>
        /// <param name="data">the data to render into this table</param>
        /// <param name="colDefs">the definitions of each column in this table</param>
        private static void AddDataToTable<T>(PdfPTable t, IEnumerable<T> data, IEnumerable<PdfColumnDef<T>> colDefs)
        {
            // step through each row in the data
            foreach (var dataRow in data)
            {
                // step through each column in the table
                foreach (var colDef in colDefs)
                {
                    // get the field for this column from this row and put it in a cell
                    object value = colDef.GetDisplayValue != null 
                        ? colDef.GetDisplayValue(dataRow) 
                        : colDef.GetFieldValue(dataRow);

                    t.AddCell(BuildCell(value, colDef));
                }

                // mark the row finished
                t.CompleteRow();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_alreadyDisposed)
            {
                if (disposing)
                {
                    _memoryStream.Dispose();
                }

                Document = null;

                _alreadyDisposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
