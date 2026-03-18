using BankService.Application.DTOs;
using MediatR;

namespace BankService.Application.Queries.BankAccounts;

public record GetAllBankAccountsQuery : IRequest<IReadOnlyList<BankAccountDto>>;

public record GetBankAccountByIdQuery(long AccountId) : IRequest<BankAccountDto?>;

public record GetBankAccountsByTrustCodeQuery(string TrustCode) : IRequest<IReadOnlyList<BankAccountDto>>;
