using AccidentManagementService.Domain.Entities;
using AccidentManagementService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AccidentManagementService.GraphQL
{
    public class Query
    {
        /// <summary>
        /// Get all accident records
        /// </summary>
        [GraphQLDescription("Retrieves all accident records from the database")]
        public async Task<List<DailyAccidentFIR>> GetAccidents(
            [Service] AccidentManagementDbContext dbContext)
        {
            return await dbContext.DailyAccidentFIRs
                .OrderByDescending(a => a.AccidentDateTime)
                .ToListAsync();
        }

        /// <summary>
        /// Get a specific accident by number
        /// </summary>
        [GraphQLDescription("Retrieves a specific accident record by accident number")]
        public async Task<DailyAccidentFIR?> GetAccident(
            decimal accidentNumber,
            [Service] AccidentManagementDbContext dbContext)
        {
            return await dbContext.DailyAccidentFIRs
                .FirstOrDefaultAsync(a => a.AccidentNumber == accidentNumber);
        }

        /// <summary>
        /// Get accidents by status
        /// </summary>
        [GraphQLDescription("Retrieves accident records filtered by status")]
        public async Task<List<DailyAccidentFIR>> GetAccidentsByStatus(
            string status,
            [Service] AccidentManagementDbContext dbContext)
        {
            return await dbContext.DailyAccidentFIRs
                .Where(a => a.Status == status)
                .OrderByDescending(a => a.AccidentDateTime)
                .ToListAsync();
        }

        /// <summary>
        /// Get accidents by company code
        /// </summary>
        [GraphQLDescription("Retrieves accident records for a specific company")]
        public async Task<List<DailyAccidentFIR>> GetAccidentsByCompany(
            string companyCode,
            [Service] AccidentManagementDbContext dbContext)
        {
            return await dbContext.DailyAccidentFIRs
                .Where(a => a.CompanyCode == companyCode)
                .OrderByDescending(a => a.AccidentDateTime)
                .ToListAsync();
        }

        /// <summary>
        /// Get accident statistics
        /// </summary>
        [GraphQLDescription("Get summary statistics of all accidents")]
        public async Task<AccidentStatistics> GetAccidentStatistics(
            [Service] AccidentManagementDbContext dbContext)
        {
            var accidents = await dbContext.DailyAccidentFIRs.ToListAsync();
            
            return new AccidentStatistics
            {
                TotalAccidents = accidents.Count,
                ReportedCount = accidents.Count(a => a.Status == "Reported"),
                InProgressCount = accidents.Count(a => a.Status == "InvestigationInProgress"),
                ClosedCount = accidents.Count(a => a.Status == "Closed"),
                PendingCount = accidents.Count(a => a.Status == "Pending"),
                LatestAccidentDate = accidents.Max(a => (DateTime?)a.AccidentDateTime)
            };
        }
    }

    public class AccidentStatistics
    {
        [GraphQLDescription("Total number of accident records")]
        public int TotalAccidents { get; set; }

        [GraphQLDescription("Number of reported accidents")]
        public int ReportedCount { get; set; }

        [GraphQLDescription("Number of accidents under investigation")]
        public int InProgressCount { get; set; }

        [GraphQLDescription("Number of closed accidents")]
        public int ClosedCount { get; set; }

        [GraphQLDescription("Number of pending accidents")]
        public int PendingCount { get; set; }

        [GraphQLDescription("Date of the most recent accident")]
        public DateTime? LatestAccidentDate { get; set; }
    }
}
