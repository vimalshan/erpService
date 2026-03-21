using ComplaintService.Application.Interfaces;
using ComplaintService.Domain.Entities;
using ComplaintService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComplaintService.Infrastructure.Repositories;

public class ComplaintRepository(ComplaintDbContext dbContext) : IComplaintRepository
{
    public async Task<ComplaintTicket?> GetByIdAsync(decimal ticketNum, CancellationToken ct = default) =>
        await dbContext.ComplaintTickets
            .Include(t => t.Action)
            .ThenInclude(a => a!.Histories)
            .Include(t => t.Escalations)
            .Include(t => t.Tasks)
            .FirstOrDefaultAsync(t => t.TicketNum == ticketNum, ct);

    public async Task<IEnumerable<ComplaintTicket>> GetByGroupAsync(decimal groupId, CancellationToken ct = default) =>
        await dbContext.ComplaintTickets
            .Where(t => t.GroupId == groupId)
            .OrderByDescending(t => t.TicketNum)
            .ToListAsync(ct);

    public async Task<IEnumerable<ComplaintTicket>> GetAllAsync(int page, int pageSize, CancellationToken ct = default) =>
        await dbContext.ComplaintTickets
            .OrderByDescending(t => t.TicketNum)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task AddAsync(ComplaintTicket ticket, CancellationToken ct = default) =>
        await dbContext.ComplaintTickets.AddAsync(ticket, ct);

    public Task UpdateAsync(ComplaintTicket ticket, CancellationToken ct = default)
    {
        dbContext.ComplaintTickets.Update(ticket);
        return Task.CompletedTask;
    }

    public async Task<decimal> GetNextTicketNumAsync(CancellationToken ct = default)
    {
        var max = await dbContext.ComplaintTickets.MaxAsync(t => (decimal?)t.TicketNum, ct);
        return (max ?? 0) + 1;
    }

    public async Task<decimal> GetNextActionNumAsync(CancellationToken ct = default)
    {
        var max = await dbContext.ComplaintActions.MaxAsync(a => (decimal?)a.ActionNum, ct);
        return (max ?? 0) + 1;
    }

    public async Task<decimal> GetNextHistoryNumAsync(CancellationToken ct = default)
    {
        var max = await dbContext.ComplaintHistories.MaxAsync(h => (decimal?)h.HistoryNum, ct);
        return (max ?? 0) + 1;
    }
}
