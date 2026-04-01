using LetTransactionService.Domain.Entities;
using LetTransactionService.Domain.Interfaces;
using LetTransactionService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LetTransactionService.Infrastructure.Repositories;

public class LetRequestRepository(LetTransactionDbContext context) : ILetRequestRepository
{
    public async Task<LetMain?> GetByIdAsync(long requestNumber, CancellationToken ct = default)
        => await context.LetMain
            .Include(l => l.SubEntries)
            .FirstOrDefaultAsync(l => l.RequestNumber == requestNumber, ct);

    public async Task<IEnumerable<LetMain>> GetByEmployeeAsync(string employeeUserId, CancellationToken ct = default)
        => await context.LetMain
            .Include(l => l.SubEntries)
            .Where(l => l.EmployeeUserId == employeeUserId)
            .OrderByDescending(l => l.RequestDate)
            .ToListAsync(ct);

    public async Task<IEnumerable<LetMain>> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
        => await context.LetMain
            .Include(l => l.SubEntries)
            .OrderByDescending(l => l.RequestDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task AddAsync(LetMain letMain, CancellationToken ct = default)
    {
        await context.LetMain.AddAsync(letMain, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(LetMain letMain, CancellationToken ct = default)
    {
        context.LetMain.Update(letMain);
        await context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(long requestNumber, CancellationToken ct = default)
        => await context.LetMain.AnyAsync(l => l.RequestNumber == requestNumber, ct);
}
