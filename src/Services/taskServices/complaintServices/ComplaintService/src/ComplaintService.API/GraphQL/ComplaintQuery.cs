using ComplaintService.Application.DTOs;
using ComplaintService.Application.Interfaces;
using ComplaintService.Infrastructure.Repositories;
using Mapster;

namespace ComplaintService.API.GraphQL;

public class ComplaintQuery
{
    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<ComplaintTicketDto>> GetComplaints(
        [Service] IComplaintRepository repo,
        CancellationToken ct)
    {
        var tickets = await repo.GetAllAsync(1, 200, ct);
        return tickets.Adapt<IEnumerable<ComplaintTicketDto>>();
    }

    public async Task<ComplaintTicketDto?> GetComplaintById(
        decimal ticketNum,
        [Service] IComplaintRepository repo,
        CancellationToken ct)
    {
        var ticket = await repo.GetByIdAsync(ticketNum, ct);
        return ticket?.Adapt<ComplaintTicketDto>();
    }

    public async Task<string> GetComplaintStatus(
        decimal ticketNum,
        [Service] DapperComplaintRepository dapperRepo,
        CancellationToken ct) =>
        await dapperRepo.GetComplaintStatusAsync(ticketNum, ct);

    public async Task<IEnumerable<ComplaintGroupDto>> GetComplaintGroups(
        [Service] IComplaintGroupRepository groupRepo,
        CancellationToken ct)
    {
        var groups = await groupRepo.GetAllAsync(ct);
        return groups.Adapt<IEnumerable<ComplaintGroupDto>>();
    }
}
