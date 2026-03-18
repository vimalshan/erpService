using AccountingService.Application.DTOs;
using MediatR;

namespace AccountingService.Application.Features.AccountDetails.Queries.GetAccountDetails;

public record GetAccountDetailsQuery(string TrustCode, DateTime? From = null, DateTime? To = null)
    : IRequest<IEnumerable<AccountDetailDto>>;
