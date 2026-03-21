using ComplaintService.Domain.Entities;

namespace ComplaintService.Application.Interfaces;

public interface IComplaintGroupRepository
{
    Task<ComplaintGroup?> GetByIdAsync(string groupId, CancellationToken ct = default);
    Task<IEnumerable<ComplaintGroup>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(ComplaintGroup group, CancellationToken ct = default);
    Task UpdateAsync(ComplaintGroup group, CancellationToken ct = default);
}
