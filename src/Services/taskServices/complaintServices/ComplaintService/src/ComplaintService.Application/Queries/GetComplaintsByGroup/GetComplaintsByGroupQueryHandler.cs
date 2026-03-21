using ComplaintService.Application.DTOs;
using ComplaintService.Application.Interfaces;
using Mapster;
using MediatR;

namespace ComplaintService.Application.Queries.GetComplaintsByGroup;

public sealed class GetComplaintsByGroupQueryHandler(
    IComplaintRepository complaintRepo) : IRequestHandler<GetComplaintsByGroupQuery, IEnumerable<ComplaintTicketDto>>
{
    public async Task<IEnumerable<ComplaintTicketDto>> Handle(GetComplaintsByGroupQuery request, CancellationToken ct)
    {
        var tickets = await complaintRepo.GetByGroupAsync(request.GroupId, ct);
        return tickets.Adapt<IEnumerable<ComplaintTicketDto>>();
    }
}
