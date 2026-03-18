using MediatR;
using CashManagement.Application.DTOs;

namespace CashManagement.Application.Queries.BankAccount;

public record GetAllBankAccountsQuery : IRequest<IEnumerable<BankAccountDto>>;
public record GetBankAccountByIdQuery(long BankAccountId) : IRequest<BankAccountDto?>;
public record GetBankBalanceQuery(long BankAccountId, DateTime AsOfDate) : IRequest<BankBalanceDto>;
public record GetBankTransactionsByAccountQuery(long BankAccountId, DateTime From, DateTime To) : IRequest<IEnumerable<BankTransactionDto>>;
