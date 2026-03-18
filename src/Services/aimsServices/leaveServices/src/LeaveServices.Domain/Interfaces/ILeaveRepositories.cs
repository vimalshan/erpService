using LeaveServices.Domain.Entities;

namespace LeaveServices.Domain.Interfaces;

public interface ILeaveDetailsRepository
{
    Task<LeaveDetails?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<LeaveDetails>> GetByEmployeeAsync(long empSysId, CancellationToken ct = default);
    Task<IEnumerable<LeaveDetails>> GetPendingAsync(CancellationToken ct = default);
    Task AddAsync(LeaveDetails entity, CancellationToken ct = default);
    Task UpdateAsync(LeaveDetails entity, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}

public interface ILeaveMasterRepository
{
    Task<LeaveMaster?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<LeaveMaster>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(LeaveMaster entity, CancellationToken ct = default);
    Task UpdateAsync(LeaveMaster entity, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}

public interface ILeaveCreditRepository
{
    Task<LeaveCredit?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<LeaveCredit>> GetByEmployeeAsync(long empSysId, int year, CancellationToken ct = default);
    Task<decimal> GetBalanceAsync(long empSysId, long leaveId, CancellationToken ct = default);
    Task AddAsync(LeaveCredit entity, CancellationToken ct = default);
    Task UpdateAsync(LeaveCredit entity, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}

public interface ILeaveApprovalRepository
{
    Task AddAsync(LeaveDetailsApproval entity, CancellationToken ct = default);
    Task<IEnumerable<LeaveDetailsApproval>> GetByDetailIdAsync(long detailId, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}

public interface ILeaveRulesRepository
{
    Task<LeaveRules?> GetByLeaveIdAsync(long leaveId, CancellationToken ct = default);
    Task<IEnumerable<LeaveRules>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(LeaveRules entity, CancellationToken ct = default);
}

public interface ICompOffRepository
{
    Task<IEnumerable<CompOffAdjust>> GetAvailableByEmployeeAsync(long empSysId, CancellationToken ct = default);
    Task AddAsync(CompOffAdjust entity, CancellationToken ct = default);
    Task UpdateAsync(CompOffAdjust entity, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}
