namespace LoanApplication.Domain.Interfaces;

/// <summary>
/// Domain service for checking loan eligibility
/// </summary>
public interface ILoanEligibilityService
{
    /// <summary>
    /// Check if employee is eligible for a loan
    /// </summary>
    Task<bool> IsEligibleAsync(long employeeId, long loanTypeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get eligibility check details
    /// </summary>
    Task<EligibilityCheckResult> GetEligibilityDetailsAsync(long employeeId, long loanTypeId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of eligibility check
/// </summary>
public class EligibilityCheckResult
{
    public bool IsEligible { get; set; }
    public int ServiceYears { get; set; }
    public int ActiveLoanCount { get; set; }
    public int MaxActiveLoans { get; set; } = 2;
    public int MinServiceYears { get; set; } = 1;
    public string? Reason { get; set; }
}
