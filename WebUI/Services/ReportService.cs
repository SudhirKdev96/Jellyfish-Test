using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebUI.Data;
using WebUI.Data.Models;
using WebUI.Data.Models.Reports;

namespace WebUI.Services
{
    /// <summary>
    /// Service for KAMS report data
    /// </summary>
    public class ReportService
    {
        protected readonly ApplicationDbContext<ApplicationUser> context;

        public ReportService(ApplicationDbContext<ApplicationUser> context)
        {
            this.context = context;
        }

        /// <summary>
        /// Get all data for the project by client report using the given report options
        /// </summary>
        /// <returns>A list of projects by client</returns>
        public async Task<List<ClientProjectRow>> GetClientProjectDataAsync(ClientProjectReportOptions opts)
        {
            var rows = await context.ClientProjectRows
                .FromSqlInterpolated($@"spClientProject @clientIdParam={opts.ClientId}, @includeAllClientsParam={opts.AllClients}")
                .ToListAsync();

            return rows;
        }
    }
}
