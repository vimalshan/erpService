namespace FinyearAPI.GraphQL.Types
{
    /// <summary>
    /// GraphQL type for Financial Year
    /// </summary>
    public class FinancialYearType
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

    /// <summary>
    /// GraphQL input type for creating financial year
    /// </summary>
    public class CreateFinancialYearInput
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    /// <summary>
    /// GraphQL input type for updating financial year
    /// </summary>
    public class UpdateFinancialYearInput
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    /// <summary>
    /// GraphQL mutation response payload
    /// </summary>
    public class FinancialYearMutationPayload
    {
        public FinancialYearType? FinancialYear { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    /// <summary>
    /// GraphQL subscription event payload
    /// </summary>
    public class FinancialYearEventPayload
    {
        public FinancialYearType FinancialYear { get; set; } = null!;
        public string EventType { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
    }
}
