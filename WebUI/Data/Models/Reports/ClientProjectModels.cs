using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Data.Models.Reports
{
    /// <summary>
    /// a model of the data used in the report
    /// </summary>
    public class ClientProjectRow
    {
        public string ClientName { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal EstimatedRevenue { get; set; }
    }

    public class ClientProjectReportOptions
    {
        public int? ClientId { get; set; }
        public bool AllClients { get; set; }
    }
}
