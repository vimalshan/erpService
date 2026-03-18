using MediatR;
using LoanApplication.Application.Queries;
using LoanApplication.API.GraphQL.Types;

namespace LoanApplication.API.GraphQL;

/// <summary>
/// GraphQL Query root type
/// </summary>
public class Query
{
    /// <summary>
    /// Get loan application by ID
    /// </summary>
    public async Task<LoanApplicationType?> GetLoanApplicationById(
        [Service] IMediator mediator,
        long id,
        CancellationToken cancellationToken)
    {
        var query = new GetLoanApplicationByIdQuery { LoanApplicationId = id };
        var result = await mediator.Send(query, cancellationToken);
        return result != null ? LoanApplicationType.FromDto(result) : null;
    }

    /// <summary>
    /// Get loan applications by employee ID
    /// </summary>
    public async Task<List<LoanApplicationType>> GetLoanApplicationsByEmployee(
        [Service] IMediator mediator,
        long employeeId,
        CancellationToken cancellationToken)
    {
        var query = new GetLoanApplicationsByEmployeeIdQuery { EmployeeId = employeeId };
        var result = await mediator.Send(query, cancellationToken);
        return result.ConvertAll(LoanApplicationType.FromDto);
    }

    /// <summary>
    /// Get all loan applications
    /// </summary>
    public async Task<List<LoanApplicationType>> GetAllLoanApplications(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetAllLoanApplicationsQuery();
        var result = await mediator.Send(query, cancellationToken);
        return result.ConvertAll(LoanApplicationType.FromDto);
    }

    /// <summary>
    /// Get pending loan applications
    /// </summary>
    public async Task<List<LoanApplicationType>> GetPendingLoanApplications(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetPendingLoanApplicationsQuery();
        var result = await mediator.Send(query, cancellationToken);
        return result.ConvertAll(LoanApplicationType.FromDto);
    }

    /// <summary>
    /// Check loan eligibility
    /// </summary>
    public async Task<EligibilityCheckType> CheckLoanEligibility(
        [Service] IMediator mediator,
        long employeeId,
        long loanTypeId,
        CancellationToken cancellationToken)
    {
        var query = new CheckLoanEligibilityQuery
        {
            EmployeeId = employeeId,
            LoanTypeId = loanTypeId
        };
        var result = await mediator.Send(query, cancellationToken);
        return new EligibilityCheckType
        {
            IsEligible = result.IsEligible,
            ServiceYears = result.ServiceYears,
            ActiveLoanCount = result.ActiveLoanCount,
            MaxActiveLoans = result.MaxActiveLoans,
            MinServiceYears = result.MinServiceYears,
            Reason = result.Reason
        };
    }
}
