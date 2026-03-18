using LoanApplication.Domain.Common;

namespace LoanApplication.Domain.Entities;

/// <summary>
/// Loan Additional entity for tracking additional loans for an employee
/// </summary>
public class LoanAdditional : Entity
{
    public long EmployeeId { get; private set; }
    public long AdditionalLoanNumber { get; private set; }
    public long LoanId { get; private set; }

    private LoanAdditional() { }

    public static LoanAdditional Create(long employeeId, long additionalLoanNumber, long loanId, long createdBy)
    {
        if (employeeId <= 0)
            throw new ArgumentException("Employee ID must be greater than 0", nameof(employeeId));

        if (additionalLoanNumber <= 0)
            throw new ArgumentException("Additional Loan Number must be greater than 0", nameof(additionalLoanNumber));

        if (loanId <= 0)
            throw new ArgumentException("Loan ID must be greater than 0", nameof(loanId));

        var now = DateTime.UtcNow;

        return new LoanAdditional
        {
            EmployeeId = employeeId,
            AdditionalLoanNumber = additionalLoanNumber,
            LoanId = loanId,
            CreatedAt = now,
            CreatedBy = createdBy,
            ModifiedAt = now,
            ModifiedBy = createdBy
        };
    }
}
