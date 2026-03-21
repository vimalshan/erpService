using ComplaintService.Application.DTOs;
using ComplaintService.Application.Interfaces;
using Mapster;
using MediatR;

namespace ComplaintService.Application.Queries.GetAllComplaints;

public sealed class GetAllComplaintsQueryHandler(
    IComplaintRepository complaintRepo) : IRequestHandler<GetAllComplaintsQuery, IEnumerable<ComplaintTicketDto>>
{
    public async Task<IEnumerable<ComplaintTicketDto>> Handle(GetAllComplaintsQuery request, CancellationToken ct)
    {
        var tickets = await complaintRepo.GetAllAsync(request.Page, request.PageSize, ct);
        return tickets.Adapt<IEnumerable<ComplaintTicketDto>>();
    }
}
