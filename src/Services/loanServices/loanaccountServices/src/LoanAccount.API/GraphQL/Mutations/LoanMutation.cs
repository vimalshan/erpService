using LoanAccount.Application.Commands;
using LoanAccount.Application.DTOs;
using MediatR;

namespace LoanAccount.API.GraphQL.Mutations;

/// <summary>
/// GraphQL Mutation type for loan operations
/// </summary>
public class LoanMutation
{
    private readonly IMediator _mediator;

    public LoanMutation(IMediator mediator)
    {
        _mediator = mediator;
    }

    [GraphQLName("createLoan")]
    public async Task<long> CreateLoan(CreateLoanRequest input, CancellationToken cancellationToken)
    {
        var command = new CreateLoanCommand(
            input.LoanAppId,
            input.EmployeeId,
            input.LoanId,
            input.GradeId,
            input.PrincipalAmount,
            input.DisbursementType,
            input.LoanDate,
            input.FirstInstallmentDate,
            input.UnitId,
            input.SubClassId,
            input.Reason,
            input.GuarantorId,
            GetCurrentUserId());

        return await _mediator.Send(command, cancellationToken);
    }

    [GraphQLName("approveLoan")]
    public async Task<bool> ApproveLoan(long loanNo, ApproveLoanRequest input, CancellationToken cancellationToken)
    {
        var command = new ApproveLoanCommand(
            loanNo,
            input.InterestRate,
            GetCurrentUserId(),
            input.ApprovalRemarks);

        return await _mediator.Send(command, cancellationToken);
    }

    [GraphQLName("disburseLoan")]
    public async Task<bool> DisburseLoan(long loanNo, decimal amount, CancellationToken cancellationToken)
    {
        var command = new DisburseLoanCommand(loanNo, amount, GetCurrentUserId());
        return await _mediator.Send(command, cancellationToken);
    }

    [GraphQLName("recordEMIPayment")]
    public async Task<bool> RecordEMIPayment(long loanNo, RecordEMIPaymentRequest input, CancellationToken cancellationToken)
    {
        var command = new RecordEMIPaymentCommand(
            input.InstallmentId,
            loanNo,
            input.PrincipalPaid,
            input.InterestPaid,
            input.PaymentDate,
            GetCurrentUserId());

        return await _mediator.Send(command, cancellationToken);
    }

    [GraphQLName("settleLoan")]
    public async Task<bool> SettleLoan(long loanNo, CancellationToken cancellationToken)
    {
        var command = new SettleLoanCommand(loanNo, GetCurrentUserId());
        return await _mediator.Send(command, cancellationToken);
    }

    [GraphQLName("closeLoan")]
    public async Task<bool> CloseLoan(long loanNo, string reason, CancellationToken cancellationToken)
    {
        var command = new CloseLoanCommand(loanNo, reason, GetCurrentUserId());
        return await _mediator.Send(command, cancellationToken);
    }

    private static long GetCurrentUserId()
    {
        // In production, extract from claims context
        return 1; // Default for demo
    }
}
