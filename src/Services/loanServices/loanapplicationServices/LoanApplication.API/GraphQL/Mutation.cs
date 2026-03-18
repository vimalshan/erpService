using MediatR;
using LoanApplication.Application.Commands;
using LoanApplication.Application.DTOs;
using LoanApplication.API.GraphQL.Types;

namespace LoanApplication.API.GraphQL;

/// <summary>
/// GraphQL Mutation root type
/// </summary>
public class Mutation
{
    /// <summary>
    /// Create a new loan application
    /// </summary>
    public async Task<LoanApplicationType?> CreateLoanApplication(
        [Service] IMediator mediator,
        CreateLoanApplicationInput input,
        CancellationToken cancellationToken)
    {
        var command = new CreateLoanApplicationCommand
        {
            EmployeeId = input.EmployeeId,
            LoanId = input.LoanId,
            AppliedBy = input.AppliedBy,
            Source = input.Source,
            Amount = input.Amount,
            Reason = input.Reason,
            GuarantorId = input.GuarantorId,
            SecondGuarantorId = input.SecondGuarantorId,
            TenureMonths = input.TenureMonths
        };

        var loanApplicationId = await mediator.Send(command, cancellationToken);

        // Fetch and return the created loan application
        var getQuery = new Application.Queries.GetLoanApplicationByIdQuery { LoanApplicationId = loanApplicationId };
        var result = await mediator.Send(getQuery, cancellationToken);
        return result != null ? LoanApplicationType.FromDto(result) : null;
    }

    /// <summary>
    /// Submit loan application for approval
    /// </summary>
    public async Task<bool> SubmitLoanApplication(
        [Service] IMediator mediator,
        long loanApplicationId,
        long submittedBy,
        CancellationToken cancellationToken)
    {
        var command = new SubmitLoanApplicationCommand
        {
            LoanApplicationId = loanApplicationId,
            SubmittedBy = submittedBy
        };
        return await mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Approve loan application
    /// </summary>
    public async Task<bool> ApproveLoanApplication(
        [Service] IMediator mediator,
        long loanApplicationId,
        long approvedBy,
        string? remarks,
        CancellationToken cancellationToken)
    {
        var command = new ApproveLoanApplicationCommand
        {
            LoanApplicationId = loanApplicationId,
            ApprovedBy = approvedBy,
            Remarks = remarks
        };
        return await mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Reject loan application
    /// </summary>
    public async Task<bool> RejectLoanApplication(
        [Service] IMediator mediator,
        long loanApplicationId,
        long rejectedBy,
        string? remarks,
        CancellationToken cancellationToken)
    {
        var command = new RejectLoanApplicationCommand
        {
            LoanApplicationId = loanApplicationId,
            RejectedBy = rejectedBy,
            Remarks = remarks
        };
        return await mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Disburse loan
    /// </summary>
    public async Task<bool> DisburseLoan(
        [Service] IMediator mediator,
        long loanApplicationId,
        long disbursingBy,
        CancellationToken cancellationToken)
    {
        var command = new DisburseLoanCommand
        {
            LoanApplicationId = loanApplicationId,
            DisbursingBy = disbursingBy
        };
        return await mediator.Send(command, cancellationToken);
    }
}

/// <summary>
/// Input type for creating loan application
/// </summary>
public class CreateLoanApplicationInput
{
    public long EmployeeId { get; set; }
    public long LoanId { get; set; }
    public long AppliedBy { get; set; }
    public string Source { get; set; } = "SLF";
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public long GuarantorId { get; set; }
    public long? SecondGuarantorId { get; set; }
    public int TenureMonths { get; set; }
}
