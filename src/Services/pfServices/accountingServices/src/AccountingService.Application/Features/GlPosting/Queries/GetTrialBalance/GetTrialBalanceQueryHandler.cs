using AccountingService.Application.Common.Interfaces;
using AccountingService.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Application.Features.GlPosting.Queries.GetTrialBalance;

public class GetTrialBalanceQueryHandler : IRequestHandler<GetTrialBalanceQuery, IEnumerable<TrialBalanceDto>>
{
    private readonly IApplicationDbContext _context;

    public GetTrialBalanceQueryHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<IEnumerable<TrialBalanceDto>> Handle(GetTrialBalanceQuery request, CancellationToken cancellationToken)
    {
        return await _context.GlPostings
            .GroupBy(g => new { g.AccountCode })
            .Select(g => new TrialBalanceDto(
                g.Key.AccountCode,
                g.First().Account != null ? g.First().Account!.MainAccountName : null,
                g.Sum(x => x.DebitAmount),
                g.Sum(x => x.CreditAmount),
                g.Sum(x => x.DebitAmount) - g.Sum(x => x.CreditAmount)))
            .ToListAsync(cancellationToken);
    }
}
