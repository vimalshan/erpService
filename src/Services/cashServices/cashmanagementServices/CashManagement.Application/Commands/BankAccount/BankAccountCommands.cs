using MediatR;
using CashManagement.Application.DTOs;

namespace CashManagement.Application.Commands.BankAccount;

public record CreateBankAccountCommand(
    long BankAccountId,
    string BankName,
    string AccountNo,
    string? Branch,
    string? AccountType,
    long CreatedBy
) : IRequest<BankAccountDto>;

public record UpdateBankAccountStatusCommand(
    long BankAccountId,
    bool IsActive,
    long UpdatedBy
) : IRequest<bool>;
