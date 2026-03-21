using ComplaintService.Domain.Entities;

namespace ComplaintService.Application.Interfaces;

public interface IComplaintRepository
{
    Task<ComplaintTicket?> GetByIdAsync(decimal ticketNum, CancellationToken ct = default);
    Task<IEnumerable<ComplaintTicket>> GetByGroupAsync(decimal groupId, CancellationToken ct = default);
    Task<IEnumerable<ComplaintTicket>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(ComplaintTicket ticket, CancellationToken ct = default);
    Task UpdateAsync(ComplaintTicket ticket, CancellationToken ct = default);
    Task<decimal> GetNextTicketNumAsync(CancellationToken ct = default);
    Task<decimal> GetNextActionNumAsync(CancellationToken ct = default);
    Task<decimal> GetNextHistoryNumAsync(CancellationToken ct = default);
}
