using LeaveServices.Domain.Entities;

namespace LeaveServices.Domain.Repositories;

public interface ILeaveRequestRepository
{
    Task<LeaveRequest?> GetByIdAsync(long reqNum, CancellationToken ct = default);
    Task<IEnumerable<LeaveRequest>> GetByEmployeeAsync(string empUserId, CancellationToken ct = default);
    Task AddAsync(LeaveRequest request, CancellationToken ct = default);
    Task UpdateAsync(LeaveRequest request, CancellationToken ct = default);
}

public interface ILeaveEncashmentRepository
{
    Task<LeaveEncashment?> GetByIdAsync(long encashmentId, CancellationToken ct = default);
    Task<IEnumerable<LeaveEncashment>> GetByEmployeeAsync(long empSysId, char? status = null, CancellationToken ct = default);
    Task AddAsync(LeaveEncashment encashment, CancellationToken ct = default);
    Task UpdateAsync(LeaveEncashment encashment, CancellationToken ct = default);
}

public interface ILossOfPayRepository
{
    Task<LossOfPay?> GetByIdAsync(long lopId, CancellationToken ct = default);
    Task<IEnumerable<LossOfPay>> GetByEmployeeAsync(long empSysId, CancellationToken ct = default);
    Task AddAsync(LossOfPay lop, CancellationToken ct = default);
}

public interface ILeaveCounterRepository
{
    Task<LeaveCounter?> GetByTypeCodeAsync(string typeCode, CancellationToken ct = default);
    Task<long> GetNextSequenceAsync(string typeCode, CancellationToken ct = default);
    Task SaveAsync(LeaveCounter counter, CancellationToken ct = default);
}
