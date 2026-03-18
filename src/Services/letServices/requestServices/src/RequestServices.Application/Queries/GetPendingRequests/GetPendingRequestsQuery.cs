using MediatR;
using RequestServices.Application.DTOs;
using RequestServices.Domain.Interfaces;

namespace RequestServices.Application.Queries.GetPendingRequests;

public record GetPendingRequestsQuery(string SupervisorUser) : IRequest<IEnumerable<PendingRequestDto>>;

public class GetPendingRequestsQueryHandler(IRequestRepository repository)
    : IRequestHandler<GetPendingRequestsQuery, IEnumerable<PendingRequestDto>>
{
    public async Task<IEnumerable<PendingRequestDto>> Handle(GetPendingRequestsQuery query, CancellationToken ct)
    {
        var requests = await repository.GetPendingBySuperviorAsync(query.SupervisorUser, ct);

        return requests.SelectMany(m => m.SubRequests
            .Where(s => s.StatusCode == 'P' || s.StatusCode == 'S')
            .Select(s => new PendingRequestDto(
                m.RequestId, m.EmployeeUser, m.RequestDate,
                s.TrainingNeed, s.StatusCode)));
    }
}
