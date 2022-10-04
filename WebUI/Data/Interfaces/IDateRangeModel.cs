using System;

namespace WebUI.Data.Interfaces
{
    /// <summary>
    /// Defines required elements of a model that includes a date range
    /// </summary>
    public interface IDateRangeModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
