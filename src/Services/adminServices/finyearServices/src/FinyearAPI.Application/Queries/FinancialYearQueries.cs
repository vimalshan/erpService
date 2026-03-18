namespace FinyearAPI.Application.Queries
{
    /// <summary>
    /// Base interface for all queries in CQRS pattern
    /// </summary>
    public interface IQuery<TResponse>
    {
    }

    /// <summary>
    /// Base interface for query handlers
    /// </summary>
    public interface IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        /// <summary>
        /// Handle the query
        /// </summary>
        Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Query to get all financial years
    /// </summary>
    public class GetAllFinancialYearsQuery : IQuery<GetAllFinancialYearsResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// Response for get all query
    /// </summary>
    public class GetAllFinancialYearsResponse
    {
        public List<FinancialYearQueryDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    /// <summary>
    /// Query to get financial year by ID
    /// </summary>
    public class GetFinancialYearByIdQuery : IQuery<FinancialYearQueryDto?>
    {
        public long Id { get; set; }
    }

    /// <summary>
    /// Query to get current active financial year
    /// </summary>
    public class GetCurrentFinancialYearQuery : IQuery<FinancialYearQueryDto?>
    {
    }

    /// <summary>
    /// Query to get financial year by name
    /// </summary>
    public class GetFinancialYearByNameQuery : IQuery<FinancialYearQueryDto?>
    {
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Query DTO for financial year
    /// </summary>
    public class FinancialYearQueryDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationInDays { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
