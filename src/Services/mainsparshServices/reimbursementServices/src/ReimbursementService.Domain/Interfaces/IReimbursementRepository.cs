using ReimbursementService.Domain.Entities;
using ReimbursementService.Domain.Enums;

namespace ReimbursementService.Domain.Interfaces;

public interface IReimbursementRepository
{
    Task<ReimbursementTransaction?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ReimbursementTransaction?> GetByRefNoAsync(string refNo, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReimbursementTransaction>> GetByEmployeeAsync(long empSysId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReimbursementTransaction>> GetByStatusAsync(ReimbursementStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReimbursementTransaction>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    Task<ReimbursementTransaction> AddAsync(ReimbursementTransaction entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(ReimbursementTransaction entity, CancellationToken cancellationToken = default);
    Task<bool> RefNoExistsAsync(string refNo, CancellationToken cancellationToken = default);
}
