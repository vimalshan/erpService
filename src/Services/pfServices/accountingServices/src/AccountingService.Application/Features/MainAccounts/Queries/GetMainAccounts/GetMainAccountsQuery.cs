using AccountingService.Application.DTOs;
using MediatR;

namespace AccountingService.Application.Features.MainAccounts.Queries.GetMainAccounts;

public record GetMainAccountsQuery() : IRequest<IEnumerable<MainAccountDto>>;
