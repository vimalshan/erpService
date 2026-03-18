using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Persistence.Repositories;

public sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context) => _context = context;

    public Task<Employee?> GetByIdAsync(long id, CancellationToken ct = default)
        => _context.Employees.FirstOrDefaultAsync(e => e.Id == id, ct);

    public Task<Employee?> GetByEmployeeNoAsync(string employeeNo, CancellationToken ct = default)
        => _context.Employees.FirstOrDefaultAsync(e => e.EmployeeNo == employeeNo, ct);

    public async Task<IReadOnlyList<Employee>> GetAllAsync(int page, int size, CancellationToken ct = default)
        => await _context.Employees.Skip((page - 1) * size).Take(size).ToListAsync(ct);

    public async Task<IReadOnlyList<Employee>> GetByUnitAsync(string unit, CancellationToken ct = default)
        => await _context.Employees.Where(e => e.Unit == unit).ToListAsync(ct);

    public Task AddAsync(Employee employee, CancellationToken ct = default)
        => _context.Employees.AddAsync(employee, ct).AsTask();

    public void Update(Employee employee) => _context.Employees.Update(employee);

    public void Remove(Employee employee) => _context.Employees.Remove(employee);

    public Task<int> CountAsync(CancellationToken ct = default)
        => _context.Employees.CountAsync(ct);
}

public sealed class PromotionRepository : IPromotionRepository
{
    private readonly ApplicationDbContext _context;

    public PromotionRepository(ApplicationDbContext context) => _context = context;

    public Task<EmployeePromotion?> GetByIdAsync(long id, CancellationToken ct = default)
        => _context.EmployeePromotions.FirstOrDefaultAsync(p => p.PromotionNo == id, ct);

    public async Task<IReadOnlyList<EmployeePromotion>> GetByEmployeeIdAsync(long employeeId, CancellationToken ct = default)
        => await _context.EmployeePromotions.Where(p => p.EmployeeId == employeeId).ToListAsync(ct);

    public Task AddAsync(EmployeePromotion promotion, CancellationToken ct = default)
        => _context.EmployeePromotions.AddAsync(promotion, ct).AsTask();

    public void Update(EmployeePromotion promotion) => _context.EmployeePromotions.Update(promotion);
}

public sealed class TransferRepository : ITransferRepository
{
    private readonly ApplicationDbContext _context;

    public TransferRepository(ApplicationDbContext context) => _context = context;

    public Task<EmployeeTransfer?> GetByIdAsync(long id, CancellationToken ct = default)
        => _context.EmployeeTransfers.FirstOrDefaultAsync(t => t.TransferId == id, ct);

    public async Task<IReadOnlyList<EmployeeTransfer>> GetByEmployeeIdAsync(long employeeId, CancellationToken ct = default)
        => await _context.EmployeeTransfers.Where(t => t.EmployeeId == employeeId).ToListAsync(ct);

    public Task AddAsync(EmployeeTransfer transfer, CancellationToken ct = default)
        => _context.EmployeeTransfers.AddAsync(transfer, ct).AsTask();

    public void Update(EmployeeTransfer transfer) => _context.EmployeeTransfers.Update(transfer);
}

public sealed class ProbationRepository : IProbationRepository
{
    private readonly ApplicationDbContext _context;

    public ProbationRepository(ApplicationDbContext context) => _context = context;

    public Task<EmployeeProbation?> GetByIdAsync(long id, CancellationToken ct = default)
        => _context.EmployeeProbations.FirstOrDefaultAsync(p => p.ProbationId == id, ct);

    public Task<EmployeeProbation?> GetByEmployeeIdAsync(long employeeId, CancellationToken ct = default)
        => _context.EmployeeProbations.FirstOrDefaultAsync(p => p.EmployeeId == employeeId, ct);

    public async Task<IReadOnlyList<EmployeeProbation>> GetOverdueAsync(CancellationToken ct = default)
        => await _context.EmployeeProbations
            .Where(p => p.DueDate < DateTime.UtcNow && p.ProbationStatus == 'P')
            .ToListAsync(ct);

    public Task AddAsync(EmployeeProbation probation, CancellationToken ct = default)
        => _context.EmployeeProbations.AddAsync(probation, ct).AsTask();

    public void Update(EmployeeProbation probation) => _context.EmployeeProbations.Update(probation);
}
