using ComplaintService.Application.DTOs;
using ComplaintService.Application.Interfaces;
using Mapster;
using MediatR;

namespace ComplaintService.Application.Queries.GetComplaintById;

public sealed class GetComplaintByIdQueryHandler(
    IComplaintRepository complaintRepo) : IRequestHandler<GetComplaintByIdQuery, ComplaintTicketDto?>
{
    public async Task<ComplaintTicketDto?> Handle(GetComplaintByIdQuery request, CancellationToken ct)
    {
        var ticket = await complaintRepo.GetByIdAsync(request.TicketNum, ct);
        return ticket?.Adapt<ComplaintTicketDto>();
    }
}
