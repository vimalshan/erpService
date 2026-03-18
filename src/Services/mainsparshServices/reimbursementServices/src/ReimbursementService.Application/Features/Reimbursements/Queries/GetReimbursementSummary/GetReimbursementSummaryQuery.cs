using AutoMapper;
using MediatR;
using ReimbursementService.Application.DTOs;
using ReimbursementService.Domain.Enums;
using ReimbursementService.Domain.Interfaces;

namespace ReimbursementService.Application.Features.Reimbursements.Queries.GetReimbursementSummary;

public sealed record GetReimbursementSummaryQuery(long? EmpSysId = null) : IRequest<IEnumerable<ReimbursementSummaryDto>>;

public sealed class GetReimbursementSummaryQueryHandler(IReimbursementRepository repository)
    : IRequestHandler<GetReimbursementSummaryQuery, IEnumerable<ReimbursementSummaryDto>>
{
    public async Task<IEnumerable<ReimbursementSummaryDto>> Handle(GetReimbursementSummaryQuery request, CancellationToken cancellationToken)
    {
        var all = await repository.GetByStatusAsync(ReimbursementStatus.Paid, cancellationToken);

        var query = request.EmpSysId.HasValue
            ? all.Where(x => x.EmpSysId == request.EmpSysId.Value)
            : all;

        return query
            .GroupBy(x => new { x.EmpSysId, Type = x.ReimType.ToString(), x.Amount.Currency })
            .Select(g => new ReimbursementSummaryDto(
                g.Key.EmpSysId,
                g.Key.Type.ToUpperInvariant(),
                g.Count(),
                g.Sum(x => x.Amount.Amount),
                g.Key.Currency))
            .ToList();
    }
}
