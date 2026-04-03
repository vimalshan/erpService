using Microsoft.EntityFrameworkCore;
using LoanApplication.Domain.Interfaces;
using LoanApplication.Domain.ValueObjects;
using LoanApplication.Infrastructure.Data;

namespace LoanApplication.Infrastructure.Services;

/// <summary>
/// Implementation of Loan Eligibility Service
/// </summary>
public class LoanEligibilityService : ILoanEligibilityService
{
    private readonly LoanApplicationDbContext _context;

    public LoanEligibilityService(LoanApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> IsEligibleAsync(long employeeId, long loanTypeId, CancellationToken cancellationToken = default)
    {
        var result = await GetEligibilityDetailsAsync(employeeId, loanTypeId, cancellationToken);
        return result.IsEligible;
    }

    public async Task<EligibilityCheckResult> GetEligibilityDetailsAsync(long employeeId, long loanTypeId, CancellationToken cancellationToken = default)
    {
        var result = new EligibilityCheckResult();

        // Get service years - For now, we'll assume this comes from HRDB
        // In a real scenario, you would query the HRDB.dbo.EMPLOYEE_MASTER table
        result.ServiceYears = 2; // Mock value - should come from HRDB

        // Count active loans (approved or disbursed)
        var approved = LoanApplicationStatus.Approve();
        var disbursed = LoanApplicationStatus.Disburse();
        result.ActiveLoanCount = await _context.LoanApplications
            .Where(x => x.EmployeeId == employeeId && (x.Status == approved || x.Status == disbursed))
            .CountAsync(cancellationToken);

        // Set default values
        result.MinServiceYears = 1;
        result.MaxActiveLoans = 2;

        // Check eligibility
        if (result.ServiceYears < result.MinServiceYears)
        {
            result.IsEligible = false;
            result.Reason = $"Employee must have at least {result.MinServiceYears} year(s) of service";
        }
        else if (result.ActiveLoanCount >= result.MaxActiveLoans)
        {
            result.IsEligible = false;
            result.Reason = $"Employee has reached maximum active loans ({result.MaxActiveLoans})";
        }
        else
        {
            result.IsEligible = true;
            result.Reason = "Employee is eligible for loan";
        }

        return result;
    }
}
