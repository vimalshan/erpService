using LoanDefinition.Application.DTOs;
using MediatR;

namespace LoanDefinition.Application.Features.Loans.Commands;

public record CreateLoanCommand(
    long LoanId, string LoanName, string LoanPurpose, long LoanTypeId,
    long MinimumLimit, long MaximumLimit, DateTime EffectiveDate,
    string RecoveryType, string CompoundingFactor, string InterestFrequency,
    long PrincipalRecoveryEdId, long InterestRecoveryEdId, long PrincipalPaymentEdId,
    long CreatedBy) : IRequest<LoanMasterDto>;

public record UpdateLoanCommand(
    long LoanId, string LoanName, string LoanPurpose,
    long MinimumLimit, long MaximumLimit, long ModifiedBy) : IRequest<LoanMasterDto>;

public record CloseLoanCommand(long LoanId, DateTime ClosureDate, long ModifiedBy) : IRequest<bool>;

public record DeleteLoanCommand(long LoanId) : IRequest<bool>;
