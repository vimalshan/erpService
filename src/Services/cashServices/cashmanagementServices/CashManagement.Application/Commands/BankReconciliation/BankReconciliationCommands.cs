using MediatR;
using CashManagement.Application.DTOs;

namespace CashManagement.Application.Commands.BankReconciliation;

public record PerformBankReconciliationCommand(
    long BankAccountId,
    decimal BankStatementBalance,
    DateOnly ReconciliationDate,
    long CreatedBy
) : IRequest<BankReconciliationDto>;
