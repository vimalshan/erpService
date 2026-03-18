using AccountingService.Application.Common.Interfaces;
using AccountingService.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Application.Features.AccountDetails.Queries.GetAccountDetails;

public class GetAccountDetailsQueryHandler : IRequestHandler<GetAccountDetailsQuery, IEnumerable<AccountDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAccountDetailsQueryHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<IEnumerable<AccountDetailDto>> Handle(GetAccountDetailsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.AccountDetails.AsQueryable()
            .Where(a => a.AcTrustCode == request.TrustCode);

        if (request.From.HasValue)
            query = query.Where(a => a.AcDocDat >= request.From.Value);
        if (request.To.HasValue)
            query = query.Where(a => a.AcDocDat <= request.To.Value);

        return await query
            .Select(a => new AccountDetailDto(a.AcSysId, a.AcTrustCode, a.AcTranCode,
                a.AcTranNo, a.AcDocNo, a.AcFinYer, a.AcDocDat, a.AcMainCode, a.AcSubCode,
                a.AcDcType, a.AcTranAmt, a.AcRefTranCode, a.AcRefTranNo, a.AcRemarks))
            .ToListAsync(cancellationToken);
    }
}
