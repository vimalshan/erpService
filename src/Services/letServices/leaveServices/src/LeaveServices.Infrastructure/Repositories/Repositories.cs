using LeaveServices.Domain.Entities;
using LeaveServices.Domain.Repositories;
using LeaveServices.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LeaveServices.Infrastructure.Repositories;

public sealed class LeaveRequestRepository : ILeaveRequestRepository
{
    private readonly LeaveDbContext _context;
    public LeaveRequestRepository(LeaveDbContext context) => _context = context;

    public Task<LeaveRequest?> GetByIdAsync(long reqNum, CancellationToken ct) =>
        _context.LeaveRequests.Include(r => r.Details)
            .FirstOrDefaultAsync(r => r.ReqNum == reqNum, ct);

    public async Task<IEnumerable<LeaveRequest>> GetByEmployeeAsync(string empUserId, CancellationToken ct) =>
        await _context.LeaveRequests.Include(r => r.Details)
            .Where(r => r.EmpUserId == empUserId)
            .OrderByDescending(r => r.ReqDate)
            .ToListAsync(ct);

    public async Task AddAsync(LeaveRequest request, CancellationToken ct) =>
        await _context.LeaveRequests.AddAsync(request, ct);

    public Task UpdateAsync(LeaveRequest request, CancellationToken ct)
    {
        _context.LeaveRequests.Update(request);
        return Task.CompletedTask;
    }
}

public sealed class LeaveEncashmentRepository : ILeaveEncashmentRepository
{
    private readonly LeaveDbContext _context;
    public LeaveEncashmentRepository(LeaveDbContext context) => _context = context;

    public Task<LeaveEncashment?> GetByIdAsync(long encashmentId, CancellationToken ct) =>
        _context.LeaveEncashments.FirstOrDefaultAsync(e => e.EncashmentId == encashmentId, ct);

    public async Task<IEnumerable<LeaveEncashment>> GetByEmployeeAsync(long empSysId, char? status, CancellationToken ct) =>
        await _context.LeaveEncashments
            .Where(e => e.EmpSysId == empSysId && (status == null || e.EncashmentStatus == status))
            .OrderByDescending(e => e.RequestDate)
            .ToListAsync(ct);

    public async Task AddAsync(LeaveEncashment encashment, CancellationToken ct) =>
        await _context.LeaveEncashments.AddAsync(encashment, ct);

    public Task UpdateAsync(LeaveEncashment encashment, CancellationToken ct)
    {
        _context.LeaveEncashments.Update(encashment);
        return Task.CompletedTask;
    }
}

public sealed class LossOfPayRepository : ILossOfPayRepository
{
    private readonly LeaveDbContext _context;
    public LossOfPayRepository(LeaveDbContext context) => _context = context;

    public Task<LossOfPay?> GetByIdAsync(long lopId, CancellationToken ct) =>
        _context.LossOfPays.FirstOrDefaultAsync(l => l.LopId == lopId, ct);

    public async Task<IEnumerable<LossOfPay>> GetByEmployeeAsync(long empSysId, CancellationToken ct) =>
        await _context.LossOfPays
            .Where(l => l.EmpSysId == empSysId)
            .OrderByDescending(l => l.LopMonth)
            .ToListAsync(ct);

    public async Task AddAsync(LossOfPay lop, CancellationToken ct) =>
        await _context.LossOfPays.AddAsync(lop, ct);
}

public sealed class LeaveCounterRepository : ILeaveCounterRepository
{
    private readonly LeaveDbContext _context;
    public LeaveCounterRepository(LeaveDbContext context) => _context = context;

    public Task<LeaveCounter?> GetByTypeCodeAsync(string typeCode, CancellationToken ct) =>
        _context.LeaveCounters.FirstOrDefaultAsync(c => c.LtTypCod == typeCode, ct);

    public async Task<long> GetNextSequenceAsync(string typeCode, CancellationToken ct)
    {
        var counter = await GetByTypeCodeAsync(typeCode, ct);
        if (counter is null)
        {
            counter = LeaveCounter.Create(typeCode);
            await _context.LeaveCounters.AddAsync(counter, ct);
        }
        return counter.Increment();
    }

    public Task SaveAsync(LeaveCounter counter, CancellationToken ct)
    {
        _context.LeaveCounters.Update(counter);
        return Task.CompletedTask;
    }
}
