namespace FinyearAPI.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Financial Year API
    /// </summary>
    public class FinancialYearDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationInDays { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO for creating financial year
    /// </summary>
    public class CreateFinancialYearDto
    {
        public long FinancialYearId { get; set; }
        public string FinancialYearName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime CloseDate { get; set; }
        public long UpdatedBy { get; set; } = 1; // Default user ID
    }

    /// <summary>
    /// DTO for updating financial year
    /// </summary>
    public class UpdateFinancialYearDto
    {
        public string FinancialYearName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime CloseDate { get; set; }
        public long UpdatedBy { get; set; } = 1; // Default user ID
    }
}
