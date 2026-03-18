using AccountingService.Application.DTOs;
using MediatR;

namespace AccountingService.Application.Features.GlPosting.Queries.GetTrialBalance;

public record GetTrialBalanceQuery() : IRequest<IEnumerable<TrialBalanceDto>>;
