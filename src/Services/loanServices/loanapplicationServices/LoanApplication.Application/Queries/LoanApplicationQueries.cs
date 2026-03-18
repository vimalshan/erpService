using MediatR;
using LoanApplication.Application.DTOs;

namespace LoanApplication.Application.Queries;

/// <summary>
/// Query to get loan application by ID
/// </summary>
public class GetLoanApplicationByIdQuery : IRequest<LoanApplicationDto?>
{
    public long LoanApplicationId { get; set; }
}

/// <summary>
/// Query to get all loan applications for an employee
/// </summary>
public class GetLoanApplicationsByEmployeeIdQuery : IRequest<List<LoanApplicationDto>>
{
    public long EmployeeId { get; set; }
}

/// <summary>
/// Query to get all loan applications
/// </summary>
public class GetAllLoanApplicationsQuery : IRequest<List<LoanApplicationDto>>
{
}

/// <summary>
/// Query to get pending loan applications
/// </summary>
public class GetPendingLoanApplicationsQuery : IRequest<List<LoanApplicationDto>>
{
}

/// <summary>
/// Query to check loan eligibility
/// </summary>
public class CheckLoanEligibilityQuery : IRequest<EligibilityCheckDto>
{
    public long EmployeeId { get; set; }
    public long LoanTypeId { get; set; }
}

/// <summary>
/// Eligibility check result DTO
/// </summary>
public class EligibilityCheckDto
{
    public bool IsEligible { get; set; }
    public int ServiceYears { get; set; }
    public int ActiveLoanCount { get; set; }
    public int MaxActiveLoans { get; set; }
    public int MinServiceYears { get; set; }
    public string? Reason { get; set; }
}
