using LoanService.Application.Common;
using LoanService.Application.DTOs;
using MediatR;

namespace LoanService.Application.Loans.Commands;

// Create Loan
public record CreateLoanCommand : IRequest<Result<LoanDto>>
{
    public long LoanNo { get; init; }
    public long MemberId { get; init; }
    public decimal LoanAmount { get; init; }
    public long LoanType { get; init; }
    public string LoanReason { get; init; } = string.Empty;
    public long CreatedBy { get; init; }
    public string? TrustCode { get; init; }
    public decimal? Rate { get; init; }
    public string? Tenure { get; init; }
}

// Approve Loan
public record ApproveLoanCommand : IRequest<Result<LoanDto>>
{
    public long LoanNo { get; init; }
    public DateTime ApprovalDate { get; init; }
}

// Close Loan
public record CloseLoanCommand : IRequest<Result<LoanDto>>
{
    public long LoanNo { get; init; }
    public DateTime ClosureDate { get; init; }
}

// Add Repayment
public record AddRepaymentCommand : IRequest<Result<RepaymentDto>>
{
    public long LoanNo { get; init; }
    public int InstallmentNo { get; init; }
    public decimal Amount { get; init; }
    public DateTime DueDate { get; init; }
}

// Make Payment
public record MakePaymentCommand : IRequest<Result<RepaymentDto>>
{
    public long LoanNo { get; init; }
    public long RepaymentId { get; init; }
    public decimal PaidAmount { get; init; }
    public DateTime PaidDate { get; init; }
}

// Add Deduction
public record AddDeductionCommand : IRequest<Result<DeductionDto>>
{
    public long LoanNo { get; init; }
    public decimal Amount { get; init; }
    public DateTime Date { get; init; }
    public decimal? ContributionId { get; init; }
}
