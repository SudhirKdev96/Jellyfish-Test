using System.Collections.Generic;

namespace WebUI.Data.Interfaces
{
    /// <summary>
    /// defines required fields to implement ISortable
    /// </summary>
    public interface ISortable
    {
        public float? SortOrder { get; set; }
    }


    public class SortableComparer : IComparer<ISortable>
    {
        public int Compare(ISortable x, ISortable y)
        {
            // x is greater
            if (x.SortOrder > y.SortOrder)
            {
                return 1;
            }
            // y is greater
            else if (x.SortOrder < y.SortOrder)
            {
                return -1;
            }
            // x and y are equal
            else
            {
                return 0;
            }
        }
    }
}
