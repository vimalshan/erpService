using LetTransactionService.Application.DTOs;
using LetTransactionService.Domain.Interfaces;
using MediatR;

namespace LetTransactionService.Application.Queries.GetLetRequests;

public class GetLetRequestsQueryHandler(ILetRequestRepository repository)
    : IRequestHandler<GetLetRequestsQuery, IEnumerable<LetSummaryDto>>
{
    public async Task<IEnumerable<LetSummaryDto>> Handle(GetLetRequestsQuery query, CancellationToken ct)
    {
        IEnumerable<Domain.Entities.LetMain> results;

        if (!string.IsNullOrEmpty(query.EmployeeUserId))
            results = await repository.GetByEmployeeAsync(query.EmployeeUserId, ct);
        else
            results = await repository.GetAllAsync(query.Page, query.PageSize, ct);

        return results.Select(r => new LetSummaryDto(
            r.RequestNumber, r.EmployeeUserId, r.SupervisorUserId,
            r.RequestDate, r.SubEntries.Count));
    }
}
