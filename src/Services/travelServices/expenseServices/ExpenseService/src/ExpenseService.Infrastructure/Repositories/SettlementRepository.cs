using ExpenseService.Domain.Entities;
using ExpenseService.Domain.Interfaces;
using ExpenseService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseService.Infrastructure.Repositories;

public class SettlementRepository : ISettlementRepository
{
    private readonly ExpenseDbContext _context;

    public SettlementRepository(ExpenseDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ExpSettlement>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.ExpSettlements.ToListAsync(ct);
    }

    public async Task<ExpSettlement> AddAsync(ExpSettlement settlement, CancellationToken ct = default)
    {
        _context.ExpSettlements.Add(settlement);
        await _context.SaveChangesAsync(ct);
        return settlement;
    }

    public async Task<IReadOnlyList<ExpSettlementReport>> GetReportsByRequestAsync(long requestNumber, CancellationToken ct = default)
    {
        return await _context.ExpSettlementReports
            .Where(r => r.RequestNumber == requestNumber)
            .ToListAsync(ct);
    }
}
