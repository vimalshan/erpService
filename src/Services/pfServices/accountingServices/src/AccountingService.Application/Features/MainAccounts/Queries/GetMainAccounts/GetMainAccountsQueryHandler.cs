using AccountingService.Application.Common.Interfaces;
using AccountingService.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Application.Features.MainAccounts.Queries.GetMainAccounts;

public class GetMainAccountsQueryHandler : IRequestHandler<GetMainAccountsQuery, IEnumerable<MainAccountDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMainAccountsQueryHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<IEnumerable<MainAccountDto>> Handle(GetMainAccountsQuery request, CancellationToken cancellationToken)
    {
        return await _context.MainAccounts
            .Select(m => new MainAccountDto(m.MainAccountCode, m.MainAccountName, m.MainAccountShrtName, m.MainClosureFlag))
            .ToListAsync(cancellationToken);
    }
}
