namespace TransactionService.Domain.Interfaces;

using TransactionService.Domain.Entities;

public interface IRequestRepository : IRepository<RequestMain>
{
    Task<RequestMain?> GetByIdWithDetailsAsync(long requestId, CancellationToken ct = default);
    Task<IEnumerable<RequestMain>> GetByLocationAsync(long locationId, CancellationToken ct = default);
    Task<IEnumerable<RequestMain>> GetByEmployeeAsync(long empSysId, CancellationToken ct = default);
    Task<long> GetNextRequestIdAsync(CancellationToken ct = default);
    Task<long> GetNextRequestSubIdAsync(CancellationToken ct = default);
    Task<long> SubmitRequestSpAsync(long requestedBy, long locationId, string unitCode,
        IEnumerable<RequestSubParam> items, CancellationToken ct = default);
}

public record RequestSubParam(
    long StationaryId, long DeptId, DateTime ExpectedDate,
    long RequestedQty, string? Remarks);
